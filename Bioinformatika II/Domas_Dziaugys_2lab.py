from Bio import SeqIO
from BioHelper import BioHelper

class Config:
    entrezQueryFormat = "Human papillomavirus type {0}[Organism]"
    mafftBatPath = "mafft.bat"
    mafftGeneratedFile = "mafft_generated.FASTA"
    concatinedFilePath = "Concatinated_HPV.fasta"
    humanPapillomavirus_type16_file = "HumanPapillomavirus_T16.fasta"
    humanPapillomavirus_type16_file_type = "fasta"
    typesFolder = "all_types"
    uniqueTypesFolder = "types_with_unique_sequences"
    typeFileName = "type"
    dangerousTypes = [16, 18, 31, 33, 39, 35, 51, 52, 53, 56, 58, 59, 66]
    notDangerousTypes = [6, 11, 40, 42, 43, 44, 57, 81]


def Run():
    helper = BioHelper()

    humanPapillomavirus_type16 = SeqIO.read(open(Config.humanPapillomavirus_type16_file),
                                            Config.humanPapillomavirus_type16_file_type)

    # Download all virus types in fasta format
    helper.BlastAllTypes(Config.entrezQueryFormat,
                         humanPapillomavirus_type16.seq,
                         Config.typeFileName,
                         Config.typesFolder,
                         Config.uniqueTypesFolder,
                         Config.dangerousTypes,
                         Config.notDangerousTypes)

    helper.ConcatFastaFiles(Config.dangerousTypes,
                            Config.notDangerousTypes,
                            Config.uniqueTypesFolder,
                            Config.typeFileName,
                            Config.concatinedFilePath)

    helper.Mafft(Config.mafftBatPath, Config.concatinedFilePath, Config.mafftGeneratedFile)

Run()
