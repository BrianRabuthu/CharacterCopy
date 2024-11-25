public class Copier
{
    private readonly ISource _source;
    private readonly IDestination _destination;

    public Copier(ISource source, IDestination destination)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _destination = destination ?? throw new ArgumentNullException(nameof(destination));
    }

    public void Copy()
    {
        char c;

        while((c = _source.ReadChar()) != '\n')
        {
            _destination.WriteChar(c);
        }
    }

    public void CopyBatch(int batchSize)
    {
        while(true)
        {
            string data = _source.ReadChars(batchSize);

            if(data.Contains('\n') )
            {
                int newLineIndex = data.IndexOf('\n');
                _destination.WriteChars(data.Substring(0, newLineIndex));
                break;
            }

            _destination.WriteChars(data);
        }
    }
}