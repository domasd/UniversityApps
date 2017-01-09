import io
import os
import Bio.Blast.NCBIXML
import math
from Bio.Align.Applications import MafftCommandline
from Bio.Blast import NCBIWWW


class Config:
    blastType = "blastp"  # Protein-protein BLAST (blastp)
    database = "swissprot"
    sequenceNumber = "NP_000468.1"
    blastXmlFile = "blast.xml"
    entrezQuery = "mammals[Organism]"
    filteredFastDataFile = "filtered_data.FASTA"
    mafftGeneratedFile = "mafft_generated.FASTA"
    minIdent = 0.4
    minCoverage = 0.8
    fragmentSize = 15
    mafftBatPath = "mafft.bat"


def Run():
    helper = BioHelper()

    # Download or use existing blast xml file with Blast from NCBI
    blast_xml = None
    if os.path.isfile(Config.blastXmlFile):
        blast_xml = open(Config.blastXmlFile, "r")
    else:
        blast_xml = helper.DownloadBlastXml(Config.entrezQuery)
        save_file = open(Config.blastXmlFile, "w")
        save_file.write(blast_xml.read())
        save_file.close()
        blast_xml.seek(0, 0)  # Reposition file pointer

    filtered_fast_data = helper.ReadAndFilterBlastXmlInFAST(blast_xml)
    file = open(Config.filteredFastDataFile, "w")
    file.write(filtered_fast_data.getvalue())
    file.close()
    filtered_fast_data.close()

    # Run blast data over Mafft and align
    align = helper.RunMafftOverSequences()

    # Get most and least complying amino acid fragments
    results = helper.FindMostAndLeastComplyingFragments(align)

    print("Highest score: " + results[0])
    print("Lowest score: " + results[1])


class BioHelper:
    def RunMafftOverSequences(self):
        mafft_cline = MafftCommandline(Config.mafftBatPath, input=Config.filteredFastDataFile)
        stdout, stderr = mafft_cline()
        with open(Config.mafftGeneratedFile, "w") as handle:
            handle.write(stdout)
        from Bio import AlignIO
        return AlignIO.read(Config.mafftGeneratedFile, "fasta")

    def FindMostAndLeastComplyingFragments(self, alignedData):
        originalSeq = alignedData[0].seq
        otherSequences = alignedData[1:]

        maxScore = 0
        maxFrom = 0
        minScore = math.inf
        minFrom = 0
        for index in range(len(originalSeq) - Config.fragmentSize + 1):
            score = 0
            for otherSeqElement in otherSequences:
                for fragmentIndex in range(index, index + Config.fragmentSize):
                    if originalSeq[fragmentIndex] == otherSeqElement.seq[fragmentIndex]:
                        score += 1
            if score > maxScore:
                maxScore = score
                maxFrom = index
            if score < minScore:
                minScore = score
                minFrom = index

        return originalSeq[maxFrom:maxFrom + Config.fragmentSize], originalSeq[minFrom:minFrom + Config.fragmentSize]

    def DownloadBlastXml(self, entrez_query):
        # Downloading data
        result_handle = NCBIWWW.qblast(Config.blastType,
                                       Config.database,
                                       Config.sequenceNumber,
                                       entrez_query=entrez_query)
        return result_handle

    def ReadAndFilterBlastXmlInFAST(self, blast_xml):
        output = io.StringIO()
        blast_record = Bio.Blast.NCBIXML.read(blast_xml)

        for alignment in blast_record.alignments:
            for hsp in alignment.hsps:  # high-scoring segment pairs
                ident = round(hsp.identities / blast_record.alignments[0].length, 2)
                query_coverage = round((1 + hsp.query_end - hsp.query_start) / blast_record.alignments[0].length, 2)
                if query_coverage >= Config.minCoverage and ident >= Config.minIdent:
                    output.write(">{0}\n{1}\n".format(alignment.title, alignment.hsps[0].sbjct))
        return output


Run()
