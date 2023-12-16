using System.Runtime.Serialization;

namespace MeuLivroDeReceitas.Exceptions.ExceptionsBase;

[Serializable]
public class MeuLivroDeReceitasException: SystemException
{
    public MeuLivroDeReceitasException(string message) : base(message) { }

    protected MeuLivroDeReceitasException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
