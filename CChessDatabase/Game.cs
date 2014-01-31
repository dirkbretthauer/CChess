#region Header
////////////////////////////////////////////////////////////////////////////// 
//    This file is part of $projectname$.
//
//    $projectname$ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    $projectname$ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with $projectname$.  If not, see <http://www.gnu.org/licenses/>.
///////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CChessDatabase
{
    public class Game
    {
        public Game()
        {
            Tags = new List<Tag>();
            White = new Player();
            Black = new Player();
        }

        public int Id { get; set; }
        public Player White { get; set; }
        public Player Black { get; set; }
        public string Event { get; set; }
        public DateTime? Date { get; set; }
        public string Result { get; set; }
        public string PgnMoves { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
