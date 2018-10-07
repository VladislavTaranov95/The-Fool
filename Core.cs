/*
 * Created by SharpDevelop.
 * User: Владислав
 * Date: 03.10.2018
 * Time: 17:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TheFool
{
	/// <summary>
	/// Description of Core.
	/// </summary>
	public class Core
	{
		private char[] signs = new char[4] { '♥', '♦', '♣', '♠' };				//массив мастей карт
		private char[] convertSign = new char[4] { 'J', 'Q', 'K', 'A' };		//массив названий карт
		private int[] values = new int[] { 6, 7, 8, 9, 10, 11, 12, 13, 14 };	//массив значений карт
		private Random rnd = new Random();
		private Card[] card = new Card[36];
		private Card trump_card = new Card();
		
		public Core()
		{
		}
		
		//Функция старта игры
		public void StartGame()
		{			
			int players = int.MinValue;
			
			//Создаем колоду
			SetCardDeck(card);
			//Показываем созданную колоду
			ShowCardDeck(card);
			Console.WriteLine("Перетасованная колода: ");
			//Перетасовываем колоду
			ShuffleCardDeck(card);
			//Показываем перетасованную колоду
			ShowCardDeck(card);
			
			//Игроков не может быть меньше 2 или больше 6, поэтому проверим не введенное пользователем число
			while(players < 2 || players > 6)
			{
				try
				{
					//Предоставляем пользователю ввести число
					Console.Write("Введите количество игроков: ");
					players = Convert.ToInt32(Console.ReadLine()); 
				}
				catch(FormatException e)
				{
					//Если пользователь ввел не целое число, то обрабатываем исключение
					Console.WriteLine(e.Message);
				}
			}
			
			//Получаем и отображаем козырную карту
			trump_card = getTrumpCard(card, players);
			Console.WriteLine("\nКозырь: " + trump_card.Value + "|" + trump_card.Sign);
			
			//Выдаем игрокам карты
			SetPlayerCard(card, players);
			//Проверяем у кого карты сильнее
			CheckCards(players);
		}
		
		//Функция вычисления козырной карты
		private Card getTrumpCard(Card[] card, int players)
		{
			int index = 0;
			
			//Так как использовать рандом мы не можем (может выдать карту, которая уже есть у игрока), то выдаем карту идущую вслед за последней картой последнего игрока.
			//Если игроков 6, то соответсвенно последняя карта у 6-го игрока и есть козырная.
			if(players == 6) index = players * 6 - 1;
			else index = players * 6;
			
			Card tr_card = new Card();
			tr_card.Sign = card[index].Sign;
			tr_card.Value = card[index].Value;
			
			return tr_card;
		}
		
		//Функция создания колоды.
		private void SetCardDeck(Card[] card)
		{			
			int index = 0;
			
			//Создаем 36 объектов типа Card, это и есть наши карты.
			//Устанавливаем масть и числовое значение каждой карты.
			for(int i = 0; i < 9; i++)
			{
				for(int j = 0; j < 4; j++, index++)
				{
					card[index] = new Card();
					card[index].Sign = signs[j];
					card[index].Value = values[i];
				}
			}
		}
		
		//Функция отображения колоды.
		private void ShowCardDeck(Card[] card)
		{
			int index = 0;
			for(int i = 0; i < 9; i++)
			{
				for(int j = 0; j < 4; j++)
				{
					//Для того, чтобы заменить числа 11, 12, 13, 14 на J, Q, K, A соотвественно, используем функцию-конвертер.
					//Чтобы не заменились числа меньше 10, делаем проверку, иначе будут пустые символы.
					if(card[index].Value > 10)
					{
						char temp = ConvertValue(card[index].Value);
						Console.Write(temp + "|" + card[index].Sign + "\t");
					}
					else Console.Write(card[index].Value + "|" + card[index].Sign + "\t");
					index++;
				}
				Console.Write("\n");
			}
			Console.Write("\n");
		}
		
		//Функция перемешивания колоды.
		private void ShuffleCardDeck(Card[] card)
		{
			//Для перемешивания колоды, используем алгоритм Фишера – Йетса.
			for (int i = card.Length - 1; i >= 1; i--)
			{
   				int j = rnd.Next(i + 1);	
   				var temp = card[j];
   				card[j] = card[i];
   				card[i] = temp;
			}
		}
		
		//Функция определения сильного игрока.
		private void CheckCards(int players)
		{		
			int index = 0;
			int max_scores = 0;
			int player_id = 0;
			int[] scores = new int[players];
			
			//Чтобы вычислить какой игрок сильнее, будем слаживать числа всех карт каждого игрока и у кого сумма больше, тот и победит.
			//Но при слаживании нужно учесть козырные карты, поэтому к числу каждой козырной карты мы будем добавлять коэффициент - число 9.
			//Почему 9? Например, карту туз креста нужно сравнить с 6 чирва (которая является козырем), и, если не учитывать козырь, то туз больше 6 (туз под номером 14, потому 14 будет больше 6)!
			//Но если добавить к 6 цифру 9, в сумме будет 15 и тогда 6 чирва (которая является козырем), будет больше крестового туза.
			//Записываем результаты сумм каждого из игроков в отдельный массив.			
			for(int i = 0; i < players; i++)
			{
				for(int j = 0; j < 6; j++)
				{
					if(card[j + index].Sign == trump_card.Sign)
						scores[i] += card[j + index].Value + 9;
					else scores[i] += card[j + index].Value;
				}
				index += 6;
			}
			
			//Вычисляем у кого больше результат.
			max_scores = scores[0];
			
			for(int i = 0; i < players; i++)
				if(scores[i] > max_scores) max_scores = scores[i];
			
			//Получаем номер этого игрока и выводим победителя.
			player_id =  Array.IndexOf(scores, max_scores) + 1;		
			Console.WriteLine("\nСильные карты у игрока №" + player_id + "\n");
		}
		
		//Функция выдачи карт игрокам.
		private void SetPlayerCard(Card[] card, int players)
		{
			int index = 0;
			
			Console.Write("\n");
			for(int i = 0; i < players; i++)
			{
				Console.Write("Игрок №" + (i + 1) + ": ");
				for(int j = 0; j < 6; j++)
				{
					//Для того, чтобы заменить числа 11, 12, 13, 14 на J, Q, K, A соотвественно, используем функцию-конвертер.
					//Чтобы не заменились числа меньше 10, делаем проверку, иначе будут пустые символы.
					if(card[index].Value > 10)
					{
						char temp = ConvertValue(card[index].Value);
						Console.Write(temp + "|" + card[index].Sign + "\t");
					}
					else Console.Write(card[index].Value + "|" + card[index].Sign + "\t");
					index++;
				}
				Console.Write("\n");
			}
		}
		
		//Функция конверта чисел 11, 12, 13, 14 на J, Q, K, A соотвественно.
		private char ConvertValue(int value)
		{
			return convertSign[value - 11];
		}
	}
}
