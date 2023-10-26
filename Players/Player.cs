using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa.Players
{
    public interface IPlayer
    {
        public string playerName { get; }
        public string playerFullName { get; }

        public void Frame();
    }

    public class Player : IPlayer
    {
        public string playerName => "motae_player";
        public string playerFullName => "Motae \"Aeralis\" Zhen";

        public void Frame()
        {

        }
    }
}
