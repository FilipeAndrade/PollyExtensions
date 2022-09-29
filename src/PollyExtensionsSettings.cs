namespace PollyExtensions
{
    /// <summary>
    /// Representação das configurações Polly
    /// </summary>
    public class PollyExtensionsSettings
    {
        /// <summary>
        /// Quantidade de tentativas que será realizada
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Tempo de espera entre cada tentativa
        /// </summary>
        public int RetryAttempt { get; set; }

        public PollyExtensionsSettings()
        {

        }

        public PollyExtensionsSettings(int retryCount, int retryAttempt)
        {
            RetryCount = retryCount;
            RetryAttempt = retryAttempt;
        }

        /// <summary>
        /// Cria uma configuração padrão quando não é informado nenhuma configuração para polly
        /// </summary>
        /// <returns>Configuração padrão</returns>
        public PollyExtensionsSettings Default()
        {
            RetryCount = 2;
            RetryAttempt = 20;

            return this;
        }
    }
}
