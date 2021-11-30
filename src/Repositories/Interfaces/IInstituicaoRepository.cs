using System.Collections.Generic;
using WebApplication2.Models;

namespace WebApplication2.Repositories.Interfaces
{
    public interface IInstituicaoRepository
    {
        IEnumerable<Instituicao> Instituicoes { get; }
        Instituicao GetInstituicaoById(string cpnj);
    }
}
