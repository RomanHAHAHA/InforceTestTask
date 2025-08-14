using MediatR;

namespace InforceTestTask.Application.Features.Urls.GetAll;

public class GetAllShortUrlsQuery : IRequest<List<ShortUrlTableDto>>;