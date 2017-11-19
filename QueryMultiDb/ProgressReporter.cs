using System;

namespace QueryMultiDb
{
    public class ProgressReporter
    {
        private readonly string _label;
        private readonly int _maximumValue;
        private readonly Action<string> _reportFunction;
        private volatile int _value;
        private volatile int _lastReportedPercentage;

        public ProgressReporter(string label, int maximumValue, Action<string> reportFunction)
        {
            _label = label;
            _maximumValue = maximumValue;
            _reportFunction = reportFunction;
            _value = 0;
            _lastReportedPercentage = -1;
            ReportProgress();
        }

        public void Increment()
        {
            _value++;
            ReportProgress();
        }

        public void Done()
        {
            _value = _maximumValue;
            ReportProgress();
        }

        private void ReportProgress()
        {
            if (!Parameters.Instance.Progress)
            {
                return;
            }

            var currentValue = _value;
            var percentage = currentValue * 100 / _maximumValue;

            if (percentage % 7 != 0 && percentage != 0 && percentage != 100)
            {
                return;
            }

            if (percentage == _lastReportedPercentage)
            {
                return;
            }

            _lastReportedPercentage = percentage;

            var text = $"{_label} : {percentage}% ({currentValue}/{_maximumValue})";
            _reportFunction(text);
        }
    }
}
