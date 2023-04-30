using System;
using System.Collections.Generic;

namespace ExpensesTracker.Views.Classes
{
  internal class Unsubscriber : IDisposable
  {
    private readonly List<IObserver<double>> _observers;
    private readonly IObserver<double> _observer;

    public Unsubscriber(List<IObserver<double>> observers, IObserver<double> observer)
    {
      _observers = observers;
      _observer = observer;
    }

    public void Dispose()
    {
      if (_observers.Contains(_observer))
      {
        _observers.Remove(_observer);
      }
    }
  }
}
