using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Codenation.Challenge.Exceptions;

namespace Codenation.Challenge
{
    public class SoccerTeamsManagerTest
    {
        private static IManageSoccerTeams EmptyFactory()
        {
            return new SoccerTeamsManager();
        }

        private static IManageSoccerTeams NoPlayerManagerFactory(long teamId)
        {
            var manager = EmptyFactory();
            manager.AddTeam(teamId, $"Time {teamId}", DateTime.Now, "cor 1", "cor 2");
            return manager;
        }

        private static IManageSoccerTeams OnePlayerManagerFactory(long teamId, long playerId)
        {
            var manager = NoPlayerManagerFactory(teamId);
            manager.AddPlayer(playerId, teamId, $"Jogador {playerId}", DateTime.Today, 0, 0);
            return manager;
        }

        public static IManageSoccerTeams SomePlayersManagerFactory(long teamId, IEnumerable<long> playersIds)
        {
            var manager = EmptyFactory();
            manager.AddTeam(teamId, $"Time {teamId}", DateTime.Now, "cor 1", "cor 2");
            playersIds.ToList().ForEach(playerId => {
                manager.AddPlayer(playerId, teamId, $"Jogador {playerId}", DateTime.Today, 0, 0);
            });            
            return manager;
        }

        [Fact]
        public void Should_Be_Unique_Ids_For_Teams()
        {
            long teamId = 1;
            var manager = EmptyFactory();
            manager.AddTeam(teamId, $"Time {teamId}", DateTime.Now, "cor 1", "cor 2");
            Assert.Throws<UniqueIdentifierException>(() =>
                manager.AddTeam(teamId, $"Time {teamId}", DateTime.Now, "cor 1", "cor 2"));
        }
 
        [Fact]
        public void Should_Be_Valid_Player_When_Set_Team_Captain()
        {
            long teamId = 1;
            long playerId = 1;
            long wrongCaptainId = 2;
            
            var manager = OnePlayerManagerFactory(teamId, playerId);
            Assert.Throws<PlayerNotFoundException>(() =>
                manager.SetCaptain(wrongCaptainId));
        }

        [Fact]
        public void Should_Be_Valid_Captain_When_Get_Team_Captain()
        {
            long teamId = 1;
            long playerId = 1;
            long captainId = 1;            

            var manager = OnePlayerManagerFactory(teamId, playerId);
            Assert.Throws<CaptainNotFoundException>(() =>
                manager.GetTeamCaptain(teamId));

            manager.SetCaptain(captainId);
            Assert.Equal(captainId, manager.GetTeamCaptain(teamId));
        }

        [Fact]
        public void Should_Ensure_Right_Value_When_Get_Team_Name()
        {
            long teamId = 1;
            long wrongTeamId = 2;

            var manager = NoPlayerManagerFactory(teamId);

            Assert.Throws<TeamNotFoundException>(() =>
                manager.GetTeamName(wrongTeamId));

            Assert.Equal($"Time {teamId}", manager.GetTeamName(teamId));
        }

        [Fact]
        public void Should_Be_Valid_Team_When_Get_Team_Players()
        {
            long teamId = 1;
            long playerId = 1;
            long wrongTeamId = 2;
            var manager = OnePlayerManagerFactory(teamId, playerId);

            Assert.Throws<TeamNotFoundException>(() => 
                manager.GetTeamPlayers(wrongTeamId));
        }

        [Fact]
        public void Should_Ensure_Sort_Order_When_Get_Team_Players()
        {
            long teamId = 1;
            var playersIds = new List<long>() {15, 2, 33, 4, 13};
            var manager = SomePlayersManagerFactory(teamId, playersIds);

            playersIds.Sort();
            Assert.Equal(playersIds, manager.GetTeamPlayers(teamId));
        }

        [Fact]
        public void Should_Be_Valid_Team_When_Get_Best_Team_Player()
        {
            long teamId = 1;
            long playerId = 1;
            long wrongTeamId = 2;
            var manager = OnePlayerManagerFactory(teamId, playerId);
            Assert.Throws<TeamNotFoundException>(() => 
                manager.GetBestTeamPlayer(wrongTeamId));
        }
     
        [Theory]
        [InlineData("10,20,300,40,50", 2)]
        [InlineData("50,240,3,1,50", 1)]
        [InlineData("10,22,24,3,24", 2)]
        public void Should_Choose_Best_Team_Player(string skills, int bestPlayerId)
        {
            long teamId = 1;
            var manager = NoPlayerManagerFactory(teamId);
            var skillsLevelList = skills.Split(',').Select(x => int.Parse(x)).ToList();
            for(int i = 0; i < skillsLevelList.Count(); i++)
                manager.AddPlayer(i, teamId, $"Jogador {i}", DateTime.Today, skillsLevelList[i], 0);

            Assert.Equal(bestPlayerId, manager.GetBestTeamPlayer(teamId));
        }

        [Fact]
        public void Should_Be_Valid_Team_When_Get_Older_Player()
        {
            long teamId = 1;
            long wrongTeamId = 2;
            var manager = NoPlayerManagerFactory(teamId);
            Assert.Throws<TeamNotFoundException>(() => 
                manager.GetOlderTeamPlayer(wrongTeamId));
        }

        [Theory]
        [InlineData("15,20,33,11,50", 4)]
        [InlineData("50,24,13,15,16", 0)]
        [InlineData("17,22,50,33,50", 2)]
        public void Should_Choose_Older_Team_Player(string ages, int olderPlayerId)
        {
            long teamId = 1;
            var manager = NoPlayerManagerFactory(teamId);
            var playersAgeList = ages.Split(',').Select(x => int.Parse(x)).ToList();
            for(int i = 0; i < playersAgeList.Count(); i++)
                manager.AddPlayer(i, teamId, $"Jogador {i}", DateTime.Today.AddYears(-1 * playersAgeList[i]), 0, 0);

            Assert.Equal(olderPlayerId, manager.GetOlderTeamPlayer(teamId));
        }
        
        [Theory]
        [InlineData("51,22,23,42,5,1")]
        [InlineData("301,781,397,882,121,344,463,688,908,547")]
        public void Should_Ensure_Sort_Order_When_Get_Teams(string teamsIds)
        {
            var manager = EmptyFactory();
            var teamsIdsList = new List<long>();
            Assert.Equal(teamsIdsList, manager.GetTeams());

            teamsIdsList = teamsIds.Split(',').Select(x => long.Parse(x)).ToList();
            foreach(var teamId in teamsIdsList)
                manager.AddTeam(teamId, $"Time {teamId}", DateTime.Now, "cor 1", "cor 2");

            teamsIdsList.Sort();
            Assert.Equal(teamsIdsList, manager.GetTeams());
        }

        [Fact]
        public void Should_Be_Valid_Team_When_Get_Higher_Salary_Player()
        {
            long teamId = 1;
            long wrongTeamId = 2;
            var manager = NoPlayerManagerFactory(teamId);
            Assert.Throws<TeamNotFoundException>(() => 
                manager.GetHigherSalaryPlayer(wrongTeamId));
        }

        [Theory]
        [InlineData("1509.10;200.20;3300;450020.11;50.0", 3)]
        [InlineData("15090;45000;3300;2000;5000", 1)]
        public void Should_Choose_Higher_Salary_Player(string salaries, int highSalaryPlayerId)
        {
            long teamId = 1;
            var manager = NoPlayerManagerFactory(teamId);
            var playersSalariesList = salaries.Split(';').Select(x => decimal.Parse(x)).ToList();
            for(int i = 0; i < playersSalariesList.Count(); i++)
                manager.AddPlayer(i, teamId, $"Jogador {i}", DateTime.Today, 0, playersSalariesList[i]);

            Assert.Equal(highSalaryPlayerId, manager.GetHigherSalaryPlayer(teamId));
        }

        [Fact]
        public void Should_Choose_Right_Salary_When_Get_Player_Salary()
        {
            long teamId = 1;
            long playerId = 1;
            long wrongPlayerId = 2;
            decimal salary = 2319.42M;

            var manager = NoPlayerManagerFactory(teamId);
            manager.AddPlayer(playerId, teamId, $"Jogador {playerId}", DateTime.Today, 0, salary);

            Assert.Throws<PlayerNotFoundException>(() => 
                manager.GetPlayerSalary(wrongPlayerId));

            Assert.Equal(salary, manager.GetPlayerSalary(playerId));
        }

        [Theory]
        [InlineData("7,24,33,2,70|10,240,73,1,50|17,220,23,14,5", 5, "7,12,8,5,10")]
        [InlineData("7,50,33,2,70|10,240,73,1,50|17,220,70,14,5", 10, "7,12,8,5,13,2,10,3,11,14")]
        public void Should_Sort_All_Players_By_Skill_When_Get_Top_Players(string skillsMap, int top, string topPlayersIds)
        {
            var manager = EmptyFactory();
            long playerId = 1;
            var teamSkillsList = skillsMap.Split('|').ToList();
            for(int i = 0; i < teamSkillsList.Count(); i++)
            {
                var teamId = i + 1;
                manager.AddTeam(teamId, $"Time {teamId}", DateTime.Now, "cor 1", "cor 2");
                var skillsList = teamSkillsList[i].Split(',').Select(x => int.Parse(x)).ToList();
                for(int j = 0; j < skillsList.Count(); j++)
                {                    
                    manager.AddPlayer(playerId, teamId, $"Jogador {playerId}", DateTime.Today, skillsList[j], 0);
                    playerId++;
                }
            }

            var topPlayersList = topPlayersIds.Split(',').Select(x => long.Parse(x)).ToList();
            Assert.Equal(topPlayersList, manager.GetTopPlayers(top));
        }

        [Theory]
        [InlineData("Azul;Vermelho", "Azul;Amarelo", "Amarelo")]
        [InlineData("Azul;Vermelho", "Amarelo;Laranja", "Amarelo")]
        [InlineData("Azul;Vermelho", "Azul;Vermelho", "Vermelho")]
        public void Should_Choose_Right_Color_When_Get_Visitor_Shirt_Color(string teamColors, string visitorColors, string visitorMatchColor)
        {
            long teamId = 1;
            long visitorTeamId = 2;
            var teamColorList = teamColors.Split(";");
            var visitorColorList = visitorColors.Split(";");

            var manager = EmptyFactory();
            manager.AddTeam(teamId, $"Time {teamId}", DateTime.Now, teamColorList[0], teamColorList[1]);
            manager.AddTeam(visitorTeamId, $"Time {visitorTeamId}", DateTime.Now, visitorColorList[0], visitorColorList[1]);

            Assert.Equal(visitorMatchColor, manager.GetVisitorShirtColor(teamId, visitorTeamId));
        }
    }
}
