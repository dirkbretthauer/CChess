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


namespace CChessCore
{
    public enum RuleResult
    {
        /// <summary>
        /// The move must be dropped immediately without checking
        /// any further rules.
        /// </summary>
        Deny = -1,

        /// <summary>
        /// This rule is neutral with respect to the move. 
        /// The remaining rules, if any, should be consulted for a final decision.
        /// </summary>
        Neutral = 0,

        /// <summary>
        /// The move can be executed without consulting
        /// further rules.
        /// </summary>
        Accept = 1,
    }
}