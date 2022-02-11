namespace Trove.DataModels.Metadata;

public class FileType
{
    public FileClass FileClass { get; set; }
    public string Extension { get; set; }

    public FileType()
    {
        
    }
    
    public FileType(FileClass fileClass, string extension)
    {
        FileClass = fileClass;
        Extension = extension;
    }
}