﻿// -----------------------------------------------
//     Author: Ramon Bollen
//      File: PlayingCards.tests.GameT.cs
// Created on: 20220724
// -----------------------------------------------

using PlayingCards.Models;

namespace PlayingCards.tests;

public class GameT
{
    [Fact]
    public void WinOnHighestHand()
    {
        Player player1 = new("Loki");
        Player player2 = new("Thor");

        player1.GiveCards("2C 10C");
        player2.GiveCards("4H 6H");

        Game game = new();
        game.AddPlayer(player1);
        game.AddPlayer(player2);

        game.GiveCards("2H 4C QS KC AD");

        game.Result().Count.Should().Be(2);

        player1.Hand.Name.Should().Be(Hand.HandName.Pair);
        player2.Hand.Name.Should().Be(Hand.HandName.Pair);

        game.Result().Count(r => r.result.Hand         == Outcome.Result.Draw).Should().Be(2);
        game.Result().Single(r => r.result.HighestHand == Outcome.Result.Wins).player.PlayerName.Should().Be("Thor");
    }

    [Fact]
    public void DrawWithHighCard()
    {
        Player player1 = new("Loki");
        Player player2 = new("Thor");

        player1.GiveCards("4C JD");
        player2.GiveCards("4S 9D");

        Game game = new();
        game.AddPlayer(player1);
        game.AddPlayer(player2);

        game.GiveCards("3H 4H 5H 6H AD");

        game.Result().Count.Should().Be(2);

        game.Result().Count(r => r.result.Hand == Outcome.Result.Draw).Should().Be(2);
        player1.Hand.Name.Should().Be(Hand.HandName.Pair);
        player2.Hand.Name.Should().Be(Hand.HandName.Pair);
        game.Result().Single(r => r.result.HighestHand == Outcome.Result.Wins).player.PlayerName.Should().Be("Loki");
    }

    [Theory]
    [InlineData("8H 3H", "4H 9H",  "2C 4D 6H 8S 10C", "PlayerA")]
    [InlineData("6H 3H", "2H 9H",  "4D 6H 8S 10C 2C", "PlayerA")]
    [InlineData("2H 3H", "10H 3D", "4D 6H 8S 10C 2C", "PlayerB")]
    public void HighPairOverLowPair(string playerACards, string playerBCards, string gameCards, string winningPlayer)
    {
        Player player1 = new("PlayerA");
        Player player2 = new("PlayerB");

        player1.GiveCards(playerACards);
        player2.GiveCards(playerBCards);

        Game game = new();
        game.AddPlayer(player1);
        game.AddPlayer(player2);
        game.GiveCards(gameCards);

        game.Result().Count.Should().Be(2);

        player1.Hand.Name.Should().Be(Hand.HandName.Pair);
        player2.Hand.Name.Should().Be(Hand.HandName.Pair);

        game.Result().Single(r => r.result.HighestHand == Outcome.Result.Wins).player.PlayerName.Should().Be(winningPlayer);
    }

    [Fact]
    public void StraightFlushOverStraight()
    {
        Player player1 = new("Loki");
        Player player2 = new("Thor");
        Player player3 = new("Odin");

        player1.GiveCards("4H 2D");
        player2.GiveCards("4C 6C");
        player3.GiveCards("8H 6C");

        Game game = new();
        game.AddPlayer(player1);
        game.AddPlayer(player2);
        game.AddPlayer(player3);

        game.GiveCards("3H 7H 5H 6H AD");

        game.Result().Count.Should().Be(1);

        player1.Hand.Name.Should().Be(Hand.HandName.StraightFlush);
        player2.Hand.Name.Should().Be(Hand.HandName.Straight);
        player3.Hand.Name.Should().Be(Hand.HandName.Flush);
        game.Result().Single(r => r.result.Hand == Outcome.Result.Wins).player.PlayerName.Should().Be("Loki");
    }

    [Fact]
    public void DrawOnHighestHand()
    {
        // ReSharper disable once StringLiteralTypo
        Player player1 = new("Skadi");
        Player player2 = new("Thor");
        Player player3 = new("Freya");

        player1.GiveCards("2C 3C");
        player2.GiveCards("2H 3H");
        player3.GiveCards("2D 3D");

        Game game = new();
        game.AddPlayer(player1);
        game.AddPlayer(player2);
        game.AddPlayer(player3);

        game.GiveCards("3S KD KC 6S 8H");

        game.Result().Count(r => r.result.Hand        == Outcome.Result.Draw).Should().Be(3);
        game.Result().Count(r => r.result.HighestHand == Outcome.Result.Draw).Should().Be(3);
    }

    [Theory]
    [InlineData("10H AH",  "2S QH KH JH AS",  Hand.HandName.RoyalFlush)]
    [InlineData("8S 4S",   "QH 6S 7S 5S KH",  Hand.HandName.StraightFlush)]
    [InlineData("5C 5S",   "AS 5H 5D 10S 2S", Hand.HandName.FourOfAKind)]
    [InlineData("10H 10D", "2S 10S 9S 9H 2C", Hand.HandName.FullHouse)]
    [InlineData("4H 6H",   "2S 10H JH QH 2C", Hand.HandName.Flush)]
    [InlineData("5D 8D",   "2S 9C 6H 7S 2C",  Hand.HandName.Straight)]
    [InlineData("5D 5S",   "2S 3C 5H 9D QC",  Hand.HandName.ThreeOfAKind)]
    [InlineData("KH 2S",   "AC 5D 10S KC 2H", Hand.HandName.TwoPair)]
    [InlineData("3C 5H",   "QD 9D 5D 2S AS",  Hand.HandName.Pair)]
    public void CheckHands(string playerCards, string gameCards, Hand.HandName expectedHand)
    {
        Player testPlayer = new(string.Empty);
        testPlayer.GiveCards(playerCards);

        Game game = new();
        game.AddPlayer(testPlayer);
        game.GiveCards(gameCards);

        testPlayer.Hand.Name.Should().Be(expectedHand);
    }
}