using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Application.Services")]
[assembly: InternalsVisibleTo("Application.UseCases")]
[assembly: InternalsVisibleTo("Infrastructure.TcpClient")]
[assembly: InternalsVisibleTo("Infrastructure.Cache")]
[assembly: InternalsVisibleTo("Configuration")]
[assembly: InternalsVisibleTo("Presentation.Controllers")]
namespace Domain.Services;