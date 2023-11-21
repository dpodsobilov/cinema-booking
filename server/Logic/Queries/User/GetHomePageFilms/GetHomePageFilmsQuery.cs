using Logic.DTO;
using MediatR;

namespace Logic.Queries.GetHomePageFilms;

public class GetHomePageFilmsQuery : IRequest<IList<HomePageDto>>
{
    
}