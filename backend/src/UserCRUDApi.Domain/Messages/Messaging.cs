namespace UserCRUDApi.Domain.Messages
{
    public static class Messaging
    {
        public static readonly string MessageGenerateToken = "Token gerado com sucesso";

        public static readonly string MessageRequiredFields = "Preencher todos os campos obrigatórios";

        public static readonly string MessageErrorUnexpectedOccurred = "Ocorreu um erro inesperado";

        public static readonly string MessageRecordNotFound = "Nenhum registro encontrado";

        public static readonly string MessageSavedSuccess = "Salvo com sucesso";

        public static readonly string MessageSavedError = "Não foi possível salvar";

        public static readonly string MessageDeletedSuccess = "Excluído com sucesso";

        public static readonly string MessageDeletedError = "Não foi possível excluir";

        public static readonly string MessageUserRegisteredWithThisEmail = "Existe um Usuário cadastrado com esse E-mail";

        public static readonly string MessageBirthDateError = "Data de nascimento não pode ser maior que hoje";

        public static readonly string MessageEducationError = "Escolaridade não existe na lista de valores permitidos";

        public static readonly string MessageServiceError = "Service Exception: ";

        public static readonly string MessageRepositoryError = "Repository Exception: ";
    }
}
