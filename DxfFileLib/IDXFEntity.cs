using System.Collections.Generic;

namespace DwgConverterLib
{
    interface IDXFEntity
    { 
        DxfColor DxfColor { get; set; }
        
        List<string> AsDXFString();

    }
}
