import os
import Bio.Blast.NCBIXML
from Bio.Align.Applications import MafftCommandline
from Bio.Blast import NCBIWWW
from Bio import SeqIO
from Bio.SeqRecord import SeqRecord
from Bio.Seq import Seq

class BioHelper:
    blastType = "blastn"
    database = "nr"

    def Mafft(self, maftPath, sequencesFilePath, outputFilePath):
        print("Starting mafft") # TODO debug, delete
        mafft_cline = MafftCommandline(maftPath, input=sequencesFilePath)
        stdout, stderr = mafft_cline()
        with open(outputFilePath, "w") as handle:
            handle.write(stdout)

    def BlastAllTypes(self,
                      entrez_query_format: str,
                      humanPapillomavirus_type16,
                      typeFileName: str,
                      typesFolderName: str,
                      uniqueTypesFolderName: str,
                      dangerousTypes: list,
                      notDangerousTypes: list):

        # create a folders for saving types if they are not present
        if not os.path.exists(typesFolderName):
            os.makedirs(typesFolderName)

        if not os.path.exists(uniqueTypesFolderName):
            os.makedirs(uniqueTypesFolderName)

        # download sequences
        for typeNumber in dangerousTypes:
            self.BlastType(typeNumber,
                           True,
                           entrez_query_format,
                           humanPapillomavirus_type16,
                           typeFileName,
                           typesFolderName,
                           uniqueTypesFolderName)
        for typeNumber in notDangerousTypes:
            self.BlastType(typeNumber,
                           False,
                           entrez_query_format,
                           humanPapillomavirus_type16,
                           typeFileName,
                           typesFolderName,
                           uniqueTypesFolderName)

    def BlastType(self,
                  typeNumber,
                  isDangerous: bool,
                  entrez_query_format,
                  humanPapillomavirus_type16,
                  typeFileName,
                  typesFolderName,
                  uniqueTypesFolderName):

        file_name = self.GetFileName(typesFolderName, typeFileName, typeNumber, isDangerous)
        file_name_unique = self.GetFileName(uniqueTypesFolderName, typeFileName, typeNumber, isDangerous)

        print("File name: " + file_name)  # TODO debug, delete
        print("Unique File name: " + file_name_unique)  # TODO debug, delete

        # if files exists - no need to download them again
        if not (os.path.isfile(file_name) and os.path.isfile(file_name_unique)):
            query = entrez_query_format.format(typeNumber)

            print("Query: " + query)  # TODO debug, delete

            blast_record = self.Blast(query, humanPapillomavirus_type16)
            self.SaveRecordInFast(blast_record, file_name, file_name_unique)

    def Blast(self, entrez_query: str, seq_to_search):
        print("Calling qblast")  # TODO debug, delete
        result_handle = NCBIWWW.qblast(self.blastType,
                                       self.database,
                                       seq_to_search,
                                       entrez_query=entrez_query,
                                       hitlist_size=1000,
                                       expect=100.0)
        return result_handle

    def SaveRecordInFast(self,
                         blast_record_xml,
                         file_name: str,
                         file_name_unique: str):

        blast_record = Bio.Blast.NCBIXML.read(blast_record_xml)

        allSequences = []
        uniqueSequences = dict([])

        for alignment in blast_record.alignments:
            firstHsp = alignment.hsps[0]

            sequenceContent = firstHsp.sbjct
            sequenceTitle = alignment.title

            sequence = SeqRecord(Seq(sequenceContent), sequenceTitle)
            allSequences.append(sequence)
            uniqueSequences[sequenceContent] = sequence

        # TODO debug, delete
        print("Number of all sequences: " + str(len(allSequences)))
        print("Number of unique sequences: " + str(len(uniqueSequences)))

        SeqIO.write(allSequences, file_name, "fasta")
        SeqIO.write(uniqueSequences.values(), file_name_unique, "fasta")

    def ConcatFastaFiles(self,
                         dangerousTypes,
                         notDangerousTypes,
                         typesFolderName,
                         typeFileName,
                         outputFilePath):
        # allSequences = []

        with open(outputFilePath, "w+") as output:
            from pathlib import Path

            for typeNumber in dangerousTypes:
                file_name = self.GetFileName(typesFolderName, typeFileName, typeNumber, True)
                output.write(Path(file_name).read_text())

            for typeNumber in notDangerousTypes:
                file_name = self.GetFileName(typesFolderName, typeFileName, typeNumber, False)
                output.write(Path(file_name).read_text())

            print("Done concatinating: " + outputFilePath) # TODO debug, delete
    def GetFileName(self,
                    typesFolderName,
                    typeFileName,
                    typeNumber,
                    isDangerous) -> str:

        file_name_format = typesFolderName + "/{0}_{1}_{2}.fasta"

        if (isDangerous):
            return file_name_format.format(typeFileName, "dangerous", typeNumber)
        else:
            return file_name_format.format(typeFileName, "notDangerous", typeNumber)
