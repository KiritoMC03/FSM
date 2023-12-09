using System.Collections.Generic;

namespace FSM.Editor.Extensions
{
    public static class RepaintExtensions
    {
        public static void Repaint(this IEnumerable<ICustomRepaintHandler> list)
        {
            foreach (ICustomRepaintHandler handler in list) handler.Repaint();
        }
    }
}