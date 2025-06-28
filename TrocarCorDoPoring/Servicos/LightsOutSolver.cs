using TrocarCorDoPoring.Models;
using TrocarCorDoPoring.Servicos.Interfaces;

namespace TrocarCorDoPoring.Servicos
{
    public class LightsOutSolver : ILightsOutSolver
    {
        private const int Size = 5;
        private const int N = Size * Size;

        public List<PuzzleStep> Resolver(IEnumerable<string> greenPositions)
        {
            // 1) Constrói o vetor b (tamanho N) com 1 onde há verde.
            var b = new int[N];
            foreach (var pos in greenPositions)
            {
                int idx = ToIndex(pos); // ex: "2C" -> (2-1)*5 + ('C'-'A')
                b[idx] = 1;
            }

            // 2) Monta a matriz A (N×N), cada coluna j representa o efeito
            //    de clicar na célula j (ela mesma + cruz).
            var A = BuildMatrix();

            // 3) Resolve A·x = b em GF(2) (eliminação de Gauss).
            var x = GaussEliminationMod2(A, b);

            // 4) Converte x (vetor de 0/1) em lista de PuzzleStep.
            var steps = new List<PuzzleStep>();
            for (int j = 0; j < N; j++)
                if (x[j] == 1)
                    steps.Add(ToStep(j));
            return steps;
        }
        private static int ToIndex(string pos)
        {
            // Espera formato "<linha><coluna>" por exemplo "2C"
            if (string.IsNullOrEmpty(pos) || pos.Length < 2)
                return -1;

            // Linha: '1'..'5'
            int row = pos[0] - '1';
            // Coluna: 'A'..'E' ou 'a'..'e'
            char c = char.ToUpper(pos[1]);
            int col = c - 'A';

            if (row < 0 || row >= Size || col < 0 || col >= Size)
                return -1;
            return row * Size + col;
        }

        private static PuzzleStep ToStep(int index)
        {
            int row = index / Size + 1;
            char col = (char)('A' + (index % Size));
            return new PuzzleStep { Linha = row, Coluna = col };
        }

        private static int[,] BuildMatrix()
        {
            var A = new int[N, N];
            for (int idx = 0; idx < N; idx++)
            {
                int r = idx / Size;
                int c = idx % Size;

                // A célula clicada
                A[idx, idx] = 1;

                // Cima
                if (r > 0)
                    A[(r - 1) * Size + c, idx] = 1;
                // Baixo
                if (r < Size - 1)
                    A[(r + 1) * Size + c, idx] = 1;
                // Esquerda
                if (c > 0)
                    A[r * Size + (c - 1), idx] = 1;
                // Direita
                if (c < Size - 1)
                    A[r * Size + (c + 1), idx] = 1;
            }
            return A;
        }

        private static int[] GaussEliminationMod2(int[,] A, int[] b)
        {
            int n = A.GetLength(0);
            // Matriz aumentada M[n][n+1]
            var M = new int[n, n + 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    M[i, j] = A[i, j] & 1;
                M[i, n] = b[i] & 1;
            }

            int row = 0;
            for (int col = 0; col < n && row < n; col++)
            {
                // Encontrar pivot
                int sel = row;
                while (sel < n && M[sel, col] == 0) sel++;
                if (sel == n) continue;  // sem pivot nesta coluna

                // Troca linhas row <-> sel
                if (sel != row)
                {
                    for (int j = col; j <= n; j++)
                    {
                        var tmp = M[row, j];
                        M[row, j] = M[sel, j];
                        M[sel, j] = tmp;
                    }
                }

                // Eliminação nas outras linhas
                for (int i = 0; i < n; i++)
                {
                    if (i != row && M[i, col] == 1)
                    {
                        for (int j = col; j <= n; j++)
                            M[i, j] ^= M[row, j]; // soma mod2 = XOR
                    }
                }
                row++;
            }

            // Extrai solução x da coluna aumentada
            var x = new int[n];
            for (int i = 0; i < n; i++)
                x[i] = M[i, n] & 1;

            return x;
        }
    }
}
