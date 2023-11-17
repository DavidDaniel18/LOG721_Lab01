using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SmallTransit.Application.Services")]
[assembly: InternalsVisibleTo("SmallTransit.Application.UseCases")]
[assembly: InternalsVisibleTo("SmallTransit.Infrastructure.TcpClient")]
[assembly: InternalsVisibleTo("SmallTransit.Infrastructure.Cache")]
[assembly: InternalsVisibleTo("SmallTransit.Configuration")]
[assembly: InternalsVisibleTo("SmallTransit.Presentation.Controllers")]
namespace SmallTransit.Domain.Services;