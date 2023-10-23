using System.Collections.Generic;
using System.Threading;

public class TokenController
{
    private readonly List<CancellationTokenSource> _cancellationTokens = new();
    
    public CancellationToken CreateCancellationToken()
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        _cancellationTokens.Add(cts);
        return cts.Token;
    }

    public void CancelTokens()
    {
        for (int i = 0; i < _cancellationTokens.Count; i++)
        {
            if(_cancellationTokens[i] != null && _cancellationTokens[i].Token.CanBeCanceled)
                _cancellationTokens[i].Cancel();
        }
    }
}
