using TrocarCorDoPoring.Models;

namespace TrocarCorDoPoring.Servicos.Interfaces
{
    public interface ILightsOutSolver
    {
        /// <summary>
        /// Recebe uma lista de coordenadas verdes e retorna
        /// a sequência de cliques para zerar o tabuleiro.
        /// </summary>
        List<PuzzleStep> Resolver(IEnumerable<string> posicoesVerdes);
    }
}
