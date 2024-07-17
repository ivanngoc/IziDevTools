using System;

namespace IziHardGames.Libs.ForTasks
{
    public class RetriesExceededException : Exception
    {
        public RetriesExceededException() : base()
        {

        }
        public RetriesExceededException(string message) : base(message)
        {
        }
    }
}