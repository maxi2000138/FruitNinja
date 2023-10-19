using System;

public interface ILoosePolicy
{
    event Action NeedLoose;
    event Action NeedLateLoose;
}