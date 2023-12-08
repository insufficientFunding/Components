using Components.VisualEditor.Models;
using Components.VisualEditor.ViewModels;
using System.Threading.Tasks;
namespace Components.VisualEditor.Services;

public interface IFileService
{
    Task SaveComponentAsync (IEditor? editor);
    
    Task<IEditor?> OpenComponentAsync ();
}
