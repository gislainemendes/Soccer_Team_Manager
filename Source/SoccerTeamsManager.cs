using System;
using System.Collections.Generic;
using Codenation.Challenge.Exceptions;

namespace Codenation.Challenge
{
    public class SoccerTeamsManager : IManageSoccerTeams
    {
        List<Team> teamList = new List<Team>();
        List<Player> playerList = new List<Player>();

        public SoccerTeamsManager()
        {
        }

        public void TeamIdAlreadyExists(Team team)// Exception usada se o id do time já existe
        {
            if (teamList.Contains(team))
            {
                throw new UniqueIdentifierException();
            }
        }

        public void PlayerIdAlreadyExists(Player player)// Exception usada se o id do jogador já existe
        {
            if (playerList.Contains(player))
            {
                throw new UniqueIdentifierException();
            }
        }

        public void TeamNotFound(Team team)//Exception usada se o time informado não existir
        {
            if (teamList.Contains(team) == false)
            {
                throw new TeamNotFoundException();
            }
        }

        public void PlayerNotFound(Player player)// Exception usada se o jogador informado não existir
        {
            if (!playerList.Contains(player))
            {
                throw new PlayerNotFoundException();
            }

        }

        public void AddTeam(long id, string name, DateTime createDate, string mainShirtColor, string secondaryShirtColor)
        {    
            TeamIdAlreadyExists(new Team(id));
            teamList.Add(new Team(id, name, createDate, mainShirtColor, secondaryShirtColor));
        }

        public void AddPlayer(long id, long teamId, string name, DateTime birthDate, int skillLevel, decimal salary)
        {
            PlayerIdAlreadyExists(new Player(id));
            TeamNotFound(new Team(teamId));
            playerList.Add(new Player(id, teamId, name, birthDate, skillLevel, salary));
        }

        public void SetCaptain(long playerId)
        {   
            PlayerNotFound(new Player(playerId));
            
            long teamId = playerList[playerList.IndexOf(new Player(playerId))].teamId;

            foreach (Player player in playerList) 
            {
                if (player.teamId == teamId) {
                    if (player.id == playerId)
                    {
                        player.isCaptain = true;
                    }
                    else
                    {
                        player.isCaptain = false;
                    }
                }
            }
        }

        public long GetTeamCaptain(long teamId)
        {
            TeamNotFound(new Team(teamId));

            foreach (Player player in playerList)
            {
                if (player.teamId == teamId) { 

                    if (player.isCaptain)
                    {
                        return player.id;
                    }
                }
            }
            throw new CaptainNotFoundException();
        }

        public string GetPlayerName(long playerId)
        {
            PlayerNotFound(new Player(playerId));
            return playerList[playerList.IndexOf(new Player(playerId))].name;
        }

        public string GetTeamName(long teamId)
        {
            Team team = new Team(teamId);
            TeamNotFound(team);
            return teamList[teamList.IndexOf(team)].name;
        }

        public List<long> GetTeamPlayers(long teamId)
        {
            Team team = new Team(teamId);
            TeamNotFound(team);

            List<long> playerIdListByTeam = new List<long>();

            foreach (Player player in playerList)
            {
                if (player.teamId == teamId)
                {
                playerIdListByTeam.Add(player.id);
                }
            }
            playerIdListByTeam.Sort();
            return playerIdListByTeam;
        }

        public long GetBestTeamPlayer(long teamId)
        {
            Team team = new Team(teamId);
            TeamNotFound(team);

            long id = playerList[0].id;
            long BestSkill = playerList[0].skillLevel;

            foreach (Player player in playerList)
            {
                if (player.teamId == teamId)
                {
                    if (player.skillLevel > BestSkill)
                    {
                        id = player.id;
                        BestSkill = player.skillLevel;
                    }
                    else if (player.skillLevel == BestSkill)
                    {
                        id = player.id < id ? player.id : id;
                    }
                }
            }
            return id;
        }

        public long GetOlderTeamPlayer(long teamId)
        {
            Team team = new Team(teamId);
            TeamNotFound(team);
            long id = long.MaxValue;
            DateTime OldestPlayer = DateTime.Now;

            foreach (Player player in playerList)
            {
                if (player.teamId == teamId)
                {
                    if (DateTime.Compare(player.birthDate, OldestPlayer) < 0)
                    {
                        id = player.id;
                        OldestPlayer = player.birthDate;
                    }
                    else if (DateTime.Compare(player.birthDate, OldestPlayer) == 0)
                    {
                        id = player.id < id ? player.id : id;
                    }
                }
            }
            return id;
        }

        public List<long> GetTeams()
        {
            List<long> IdOfTeamList = new List<long>();

            foreach (Team team in teamList)
            {
                IdOfTeamList.Add(team.id);
            }

            IdOfTeamList.Sort();
            return IdOfTeamList;
        }

        public long GetHigherSalaryPlayer(long teamId)
        {
            Team team = new Team(teamId);
            TeamNotFound(team);
            long id = playerList[0].id;
            decimal highestSalary = playerList[0].salary;

            foreach (Player player in playerList)
            {
                if (player.teamId == teamId)
                {
                    if (player.salary > highestSalary)
                    {
                        id = player.id;
                        highestSalary = player.salary;
                    }
                    else if (player.salary == highestSalary)
                    {
                        id = player.id < id ? player.id : id;
                    }
                }
            }
            return id;
        }

        public decimal GetPlayerSalary(long playerId)
        {
            PlayerNotFound(new Player(playerId));
            return playerList[playerList.IndexOf(new Player(playerId))].salary;
        }

        public List<long> GetTopPlayers(int top)
        {
            List<long> TopPlayersList = new List<long>();

            playerList.Sort(delegate (Player x, Player y)
            {
                return y.skillLevel.CompareTo(x.skillLevel);
            });

            for (int i = 0; i < top; i++)
            {
                TopPlayersList.Add(playerList[i].id);
            }
            return TopPlayersList;
        }

        public string GetVisitorShirtColor(long teamId, long visitorTeamId)
        {
            Team FirstTeam = teamList[teamList.IndexOf(new Team(teamId))];
            Team SecondTeam = teamList[teamList.IndexOf(new Team(visitorTeamId))];

            TeamNotFound(FirstTeam);
            TeamNotFound(SecondTeam);

            if (FirstTeam.mainShirtColor == SecondTeam.mainShirtColor)
            {
                return SecondTeam.secondaryShirtColor;
            } 
            else
            {
                return SecondTeam.mainShirtColor;
            }
        }
    }
}
