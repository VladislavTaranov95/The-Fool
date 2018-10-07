/*
 * Created by SharpDevelop.
 * User: Владислав
 * Date: 01.10.2018
 * Time: 23:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;

namespace TheFool
{
	class Program
	{	
		public static void Main(string[] args)
		{
			//Создаем объект типа Core
			Core game = new Core();
			//Вызываем метод начала игры
			game.StartGame();
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}	
	}
}