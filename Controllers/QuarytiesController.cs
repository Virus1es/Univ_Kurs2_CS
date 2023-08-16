using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.Data.SqlClient;
using System.Data;
using Npgsql;
using Wpf_Kurvovaya_BD.Views;
using System.IO.Packaging;
//using DocumentFormat.OpenXml;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
using System.Runtime.InteropServices;

namespace Wpf_Kurvovaya_BD.Controllers;

class QuarytiesController
{
    public static PostgresContext db = new PostgresContext();

    // строка подключения
    public static string connectionString = 
        @"Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=781227xy";

    #region Show
    // достаём для показа таблицу Участие в играх
    public static List<GameIns> ShowTableGameIns() =>
        db.GameIns.OrderBy(g => g.Id).ToList();

    // достаём для показа таблицу Игры
    public static List<Games> ShowTableGames() =>
        db.Games.OrderBy(g => g.Id).ToList();

    // достаём для показа таблицу Наши клубы
    public static List<OurClubs> ShowTableOurClubs() =>
        db.OurClubs.OrderBy(g => g.Id).ToList();

    // достаём для показа таблицу Клубы противника
    public static List<EnemyClubs> ShowTableEnemyClubs() => 
        db.EnemyClubs.OrderBy(g => g.Id).ToList();

    // достаём для показа таблицу Игроки
    public static List<Gamers> ShowTableGamers() =>
        db.Gamers.OrderBy(g => g.Id).ToList();

    // достаём для показа таблицу Страны
    public static List<Countries> ShowTableCountries() => 
        db.Countries.OrderBy(g => g.Id).ToList();
    
    // достаём для показа таблицу Города
    public static List<Cities> ShowTableCities() =>
        db.Cities.OrderBy(g => g.Id).ToList();

    // достаём для показа таблицу Уровни игры
    public static List<GameLevels> ShowTableLevels() =>
        db.GameLevels.OrderBy(g => g.Id).ToList();

    // достаём для показа таблицу Позиции на поле
    public static List<Positions> ShowTablePositions() =>
        db.Positions.OrderBy(g => g.Id).ToList();

    // достаём для показа таблицу Лиги
    public static List<Leages> ShowTableLeages() =>
        db.Leages.OrderBy(g => g.Id).ToList();

    // достаём для показа таблицу Руководители клубов
    public static List<Managers> ShowTableManagers() =>
        db.Managers.OrderBy(g => g.Id).ToList();

    // достаём для показа таблицу Тренировочные базы
    public static List<TrainingBasis> ShowTableTrainingBasis() =>
        db.TrainingBases.OrderBy(g => g.Id).ToList();
    #endregion


    #region Ascending

    // сортировка GameIns по полю в зависимости от индекса
    public static List<GameIns> SortGameInsAsc(int index)
    {
        List<GameIns> gameIns = ShowTableGameIns();
        
        List<Games> games = ShowTableGames();
        List<Gamers> gamers = ShowTableGamers();
        List<OurClubs> ourClubs = ShowTableOurClubs();
        List<EnemyClubs> enemyClubs = ShowTableEnemyClubs();

        return index switch
        {
            0 => gameIns.OrderBy(g => gamers[g.IdGamer - 1].Surname).ToList(),
            1 => gameIns.OrderBy(g => gamers[g.IdGamer - 1].Name).ToList(),
            2 => gameIns.OrderBy(g => gamers[g.IdGamer - 1].Patronymic).ToList(),
            3 => gameIns.OrderBy(g => games[g.IdGame - 1].DateGame).ToList(),
            4 => gameIns.OrderBy(g => enemyClubs[games[g.IdGame - 1].IdEnemyClub - 1].Opposing).ToList(),
            5 => gameIns.OrderBy(g => ourClubs[games[g.IdGame - 1].IdOurClub - 1].Club).ToList(),
            6 => gameIns.OrderBy(g => g.Order).ToList(),
            7 => gameIns.OrderBy(g => g.CountStart).ToList(),
            8 => gameIns.OrderBy(g => g.Salary).ToList(),
            _ => gameIns
        };
    }

    // сортировка Games по полю в зависимости от индекса
    public static List<Games> SortGamesAsc(int index)
    {
        List<Games> games = ShowTableGames();
        
        List<GameIns> gameIns = ShowTableGameIns();
        List<Countries> countries = ShowTableCountries();
        List<GameLevels> levels = ShowTableLevels();
        List<OurClubs> ourClubs = ShowTableOurClubs();
        List<EnemyClubs> enemyClubs = ShowTableEnemyClubs();

        return index switch
        {
            0 => games.OrderBy(g => g.DateGame).ToList(),
            1 => games.OrderBy(g => countries[g.IdCountry - 1].Country).ToList(),
            2 => games.OrderBy(g => levels[g.IdLevel - 1].GameLevel).ToList(),
            3 => games.OrderBy(g => g.CountFinish).ToList(),
            4 => games.OrderBy(g => enemyClubs[g.IdEnemyClub - 1].Opposing).ToList(),
            5 => games.OrderBy(g => ourClubs[g.IdOurClub - 1].Club).ToList(),
            _ => games
        };
    }

    // сортировка Gamers по полю в зависимости от индекса
    public static List<Gamers> SortGamersAsc(int index)
    {
        List<Gamers> gamers = ShowTableGamers();

        List<Positions> positions = ShowTablePositions();
        List<OurClubs> ourClubs = ShowTableOurClubs();

        return index switch
        {
            0 => gamers.OrderBy(g => g.Surname).ToList(),
            1 => gamers.OrderBy(g => g.Name).ToList(),
            2 => gamers.OrderBy(g => g.Patronymic).ToList(),
            3 => gamers.OrderBy(g => positions[g.IdPosition - 1].Position).ToList(),
            4 => gamers.OrderBy(g => g.Birthday).ToList(),
            5 => gamers.OrderBy(g => ourClubs[g.IdClub - 1].Club).ToList(),
            6 => gamers.OrderBy(g => g.YearFact).ToList(),
            7 => gamers.OrderBy(g => g.Comments).ToList(),
            8 => gamers.OrderBy(g => g.Cost).ToList(),
            _ => gamers
        };
    }

    // сортировка OurClubs по полю в зависимости от индекса
    public static List<OurClubs> SortOurClubsAsc(int index)
    {
        List<OurClubs> ourClubs = ShowTableOurClubs();

        List<TrainingBasis> bases = ShowTableTrainingBasis();
        List<Managers> managers = ShowTableManagers();
        List<Cities> cities = ShowTableCities();
        List<Leages> leages = ShowTableLeages();


        return index switch
        {
            0 => ourClubs.OrderBy(c => c.Club).ToList(),
            1 => ourClubs.OrderBy(c => bases[c.IdBase - 1].Base).ToList(),
            2 => ourClubs.OrderBy(c => c.Year).ToList(),
            3 => ourClubs.OrderBy(c => leages[c.IdLeage - 1].Leage).ToList(),
            4 => ourClubs.OrderBy(c => managers[c.IdManager - 1].Surname).ToList(),
            5 => ourClubs.OrderBy(c => managers[c.IdManager - 1].Name).ToList(),
            6 => ourClubs.OrderBy(c => managers[c.IdManager - 1].Patronymic).ToList(),
            7 => ourClubs.OrderBy(c => managers[c.IdManager - 1].Phone).ToList(),
            8 => ourClubs.OrderBy(c => cities[c.IdCity - 1].City).ToList(),
            _ => ourClubs
        };
    }

    // сортировка EnemyClubs по полю в зависимости от индекса
    public static List<EnemyClubs> SortEnemyClubsAsc(int index)
    {
        List<EnemyClubs> enemyClubs = ShowTableEnemyClubs();

        List<Countries> countries = ShowTableCountries();

        return index switch
        {
            0 => enemyClubs.OrderBy(c => c.Opposing).ToList(),
            1 => enemyClubs.OrderBy(c => countries[c.IdCountry - 1].Country).ToList(),
            2 => enemyClubs.OrderBy(c => c.SurnameCoach).ToList(),
            3 => enemyClubs.OrderBy(c => c.NameCoach).ToList(),
            4 => enemyClubs.OrderBy(c => c.PatronymicCoach).ToList(),
            _ => enemyClubs
        };
    }

    #endregion


    #region Descending

    // сортировка GameIns по полю в зависимости от индекса
    public static List<GameIns> SortGameInsDesc(int index)
    {
        List<GameIns> gameIns = ShowTableGameIns();
        
        List<Games> games = ShowTableGames();
        List<Gamers> gamers = ShowTableGamers();
        List<OurClubs> ourClubs = ShowTableOurClubs();
        List<EnemyClubs> enemyClubs = ShowTableEnemyClubs();

        return index switch
        {
            0 => gameIns.OrderByDescending(g => gamers[g.IdGamer - 1].Surname).ToList(),
            1 => gameIns.OrderByDescending(g => gamers[g.IdGamer - 1].Name).ToList(),
            2 => gameIns.OrderByDescending(g => gamers[g.IdGamer - 1].Patronymic).ToList(),
            3 => gameIns.OrderByDescending(g => games[g.IdGame - 1].DateGame).ToList(),
            4 => gameIns.OrderByDescending(g => enemyClubs[games[g.IdGame - 1].IdEnemyClub - 1].Opposing).ToList(),
            5 => gameIns.OrderByDescending(g => ourClubs[games[g.IdGame - 1].IdOurClub - 1].Club).ToList(),
            6 => gameIns.OrderByDescending(g => g.Order).ToList(),
            7 => gameIns.OrderByDescending(g => g.CountStart).ToList(),
            8 => gameIns.OrderByDescending(g => g.Salary).ToList(),
            _ => gameIns
        };
    }

    // сортировка Games по полю в зависимости от индекса
    public static List<Games> SortGamesDesc(int index)
    {
        List<Games> games = ShowTableGames();
        
        List<GameIns> gameIns = ShowTableGameIns();
        List<Countries> countries = ShowTableCountries();
        List<GameLevels> levels = ShowTableLevels();
        List<OurClubs> ourClubs = ShowTableOurClubs();
        List<EnemyClubs> enemyClubs = ShowTableEnemyClubs();

        return index switch
        {
            0 => games.OrderByDescending(g => g.DateGame).ToList(),
            1 => games.OrderByDescending(g => countries[g.IdCountry - 1].Country).ToList(),
            2 => games.OrderByDescending(g => levels[g.IdLevel - 1].GameLevel).ToList(),
            3 => games.OrderByDescending(g => g.CountFinish).ToList(),
            4 => games.OrderByDescending(g => enemyClubs[g.IdEnemyClub - 1].Opposing).ToList(),
            5 => games.OrderByDescending(g => ourClubs[g.IdOurClub - 1].Club).ToList(),
            _ => games
        };
    }

    // сортировка Gamers по полю в зависимости от индекса
    public static List<Gamers> SortGamersDesc(int index)
    {
        List<Gamers> gamers = ShowTableGamers();

        List<Positions> positions = ShowTablePositions();
        List<OurClubs> ourClubs = ShowTableOurClubs();

        return index switch
        {
            0 => gamers.OrderByDescending(g => g.Surname).ToList(),
            1 => gamers.OrderByDescending(g => g.Name).ToList(),
            2 => gamers.OrderByDescending(g => g.Patronymic).ToList(),
            3 => gamers.OrderByDescending(g => positions[g.IdPosition - 1].Position).ToList(),
            4 => gamers.OrderByDescending(g => g.Birthday).ToList(),
            5 => gamers.OrderByDescending(g => ourClubs[g.IdClub - 1].Club).ToList(),
            6 => gamers.OrderByDescending(g => g.YearFact).ToList(),
            7 => gamers.OrderByDescending(g => g.Comments).ToList(),
            8 => gamers.OrderByDescending(g => g.Cost).ToList(),
            _ => gamers
        };
    }

    // сортировка OurClubs по полю в зависимости от индекса
    public static List<OurClubs> SortOurClubsDesc(int index)
    {
        List<OurClubs> ourClubs = ShowTableOurClubs();

        List<TrainingBasis> bases = ShowTableTrainingBasis();
        List<Managers> managers = ShowTableManagers();
        List<Cities> cities = ShowTableCities();
        List<Leages> leages = ShowTableLeages();


        return index switch
        {
            0 => ourClubs.OrderByDescending(c => c.Club).ToList(),
            1 => ourClubs.OrderByDescending(c => bases[c.IdBase - 1].Base).ToList(),
            2 => ourClubs.OrderByDescending(c => c.Year).ToList(),
            3 => ourClubs.OrderByDescending(c => leages[c.IdLeage - 1].Leage).ToList(),
            4 => ourClubs.OrderByDescending(c => managers[c.IdManager - 1].Surname).ToList(),
            5 => ourClubs.OrderByDescending(c => managers[c.IdManager - 1].Name).ToList(),
            6 => ourClubs.OrderByDescending(c => managers[c.IdManager - 1].Patronymic).ToList(),
            7 => ourClubs.OrderByDescending(c => managers[c.IdManager - 1].Phone).ToList(),
            8 => ourClubs.OrderByDescending(c => cities[c.IdCity - 1].City).ToList(),
            _ => ourClubs
        };
    }

    // сортировка EnemyClubs по полю в зависимости от индекса
    public static List<EnemyClubs> SortEnemyClubsDesc(int index)
    {
        List<EnemyClubs> enemyClubs = ShowTableEnemyClubs();

        List<Countries> countries = ShowTableCountries();

        return index switch
        {
            0 => enemyClubs.OrderByDescending(c => c.Opposing).ToList(),
            1 => enemyClubs.OrderByDescending(c => countries[c.IdCountry - 1].Country).ToList(),
            2 => enemyClubs.OrderByDescending(c => c.SurnameCoach).ToList(),
            3 => enemyClubs.OrderByDescending(c => c.NameCoach).ToList(),
            4 => enemyClubs.OrderByDescending(c => c.PatronymicCoach).ToList(),
            _ => enemyClubs
        };
    }

    #endregion


    #region Remove_Group

    // удаление группы записей GameIns по полю 
    public static bool DeleteGameInsByCol(int index, string value)
    {
        try
        {
            List<GameIns> gameIns = ShowTableGameIns();

            List<Games> games = ShowTableGames();
            List<Gamers> gamers = ShowTableGamers();
            List<OurClubs> ourClubs = ShowTableOurClubs();
            List<EnemyClubs> enemyClubs = ShowTableEnemyClubs();

            // найти все записи подходящие для удаления
            var deletes = index switch
            {
                0 => gameIns.Where(g => gamers.First(gamer => g.IdGamer == gamer.Id).Surname.StartsWith(value)).ToList(),
                1 => gameIns.Where(g => gamers.First(gamer => g.IdGamer == gamer.Id).Name.StartsWith(value)).ToList(),
                2 => gameIns.Where(g => gamers.First(gamer => g.IdGamer == gamer.Id).Patronymic.StartsWith(value)).ToList(),
                3 => gameIns.Where(g => games.First(game => g.IdGame == game.Id).DateGame == DateOnly.Parse(value)).ToList(),
                4 => gameIns.Where(g => enemyClubs.First(club => games.First(game => g.IdGame == game.Id).IdEnemyClub == club.Id).Opposing.StartsWith(value)).ToList(),
                5 => gameIns.Where(g => ourClubs.First(club => games.First(game => g.IdGame == game.Id).IdOurClub == club.Id).Club.StartsWith(value)).ToList(),
                6 => gameIns.Where(g => g.Order == (value == "участвовал" ? true : false)).ToList(),
                7 => gameIns.Where(g => g.CountStart == int.Parse(value)).ToList(),
                8 => gameIns.Where(g => g.Salary == long.Parse(value)).ToList(),
                _ => null
            };

            // если подтвердили удаление то выполняем
            // иначе ничего не делаем
            if (MessageBox.Show($"Вы точно хотите удалить {deletes.Count} записей?",
                                "Подтверждение",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return false;

            // удалить записи
            foreach (var item in deletes)
            {
                // удалить выбранную запись
                db.Entry(item).State = EntityState.Deleted;
                db.GameIns.Remove(item);
            }

            // записать в базу данных
            db.SaveChanges();

            
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return true;
    }

    // удаление группы записей Games по полю 
    public static bool DeleteGamesByCol(int index, string value)
    {
        try
        {
            List<Games> games = ShowTableGames();

            List<GameIns> gameIns = ShowTableGameIns();
            List<Countries> countries = ShowTableCountries();
            List<GameLevels> levels = ShowTableLevels();
            List<OurClubs> ourClubs = ShowTableOurClubs();
            List<EnemyClubs> enemyClubs = ShowTableEnemyClubs();

            // найти все записи подходящие для удаления
            var deletes = index switch
            {
                0 => games.Where(g => g.DateGame == DateOnly.Parse(value)).ToList(),
                1 => games.Where(g => countries.First(coun => g.IdCountry == coun.Id).Country.StartsWith(value)).ToList(),
                2 => games.Where(g => levels.First(l => g.IdLevel == l.Id).GameLevel.StartsWith(value)).ToList(),
                3 => games.Where(g => g.CountFinish == int.Parse(value)).ToList(),
                4 => games.Where(g => enemyClubs.First(c => g.IdEnemyClub == c.Id).Opposing.StartsWith(value)).ToList(),
                5 => games.Where(g => ourClubs.First(c => g.IdOurClub == c.Id).Club.StartsWith(value)).ToList(),
                _ => null
            };

            // если подтвердили удаление то выполняем
            // иначе ничего не делаем
            if (MessageBox.Show($"Вы точно хотите удалить {deletes.Count} записей?",
                                "Подтверждение",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return false;

            // удалить записи
            foreach (var item in deletes)
            {
                // удалить выбранную запись
                db.Entry(item).State = EntityState.Deleted;
                db.Games.Remove(item);
            }

            // записать в базу данных
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return true;
    }

    // удаление группы записей Gamers по полю 
    public static bool DeleteGamersByCol(int index, string value)
    {
        try
        {
            List<Gamers> gamers = ShowTableGamers();

            List<Positions> positions = ShowTablePositions();
            List<OurClubs> ourClubs = ShowTableOurClubs();

            // найти все записи подходящие для удаления
            var deletes = index switch
            {
                0 => gamers.Where(g => g.Surname.StartsWith(value)).ToList(),
                1 => gamers.Where(g => g.Name.StartsWith(value)).ToList(),
                2 => gamers.Where(g => g.Patronymic.StartsWith(value)).ToList(),
                3 => gamers.Where(g => positions.First(p => g.IdPosition == p.Id).Position.StartsWith(value)).ToList(),
                4 => gamers.Where(g => g.Birthday == DateOnly.Parse(value)).ToList(),
                5 => gamers.Where(g => ourClubs.First(c => g.IdClub == c.Id).Club.StartsWith(value)).ToList(),
                6 => gamers.Where(g => g.YearFact == int.Parse(value)).ToList(),
                7 => gamers.Where(g => g.Comments.StartsWith(value)).ToList(),
                8 => gamers.Where(g => g.Cost == int.Parse(value)).ToList(),
                _ => null
            };

            // если подтвердили удаление то выполняем
            // иначе ничего не делаем
            if (MessageBox.Show($"Вы точно хотите удалить {deletes.Count} записей?",
                                "Подтверждение",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return false;

            // удалить записи
            foreach (var item in deletes)
            {
                // удалить выбранную запись
                db.Entry(item).State = EntityState.Deleted;
                db.Gamers.Remove(item);
            }

            // записать в базу данных
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return true;
    }

    // удаление группы записей OurClubs по полю 
    public static bool DeleteOurClubsByCol(int index, string value)
    {
        try
        {
            List<OurClubs> ourClubs = ShowTableOurClubs();

            List<TrainingBasis> bases = ShowTableTrainingBasis();
            List<Managers> managers = ShowTableManagers();
            List<Cities> cities = ShowTableCities();
            List<Leages> leages = ShowTableLeages();

            // найти все записи подходящие для удаления
            var deletes = index switch
            {
                0 => ourClubs.Where(c => c.Club.StartsWith(value)).ToList(),
                1 => ourClubs.Where(c => bases.First(b => c.IdBase == b.Id).Base.StartsWith(value)).ToList(),
                2 => ourClubs.Where(c => c.Year == int.Parse(value)).ToList(),
                3 => ourClubs.Where(c => leages.First(l => c.IdLeage == l.Id).Leage.StartsWith(value)).ToList(),
                4 => ourClubs.Where(c => managers.First(m => c.IdManager == m.Id).Surname.StartsWith(value)).ToList(),
                5 => ourClubs.Where(c => managers.First(m => c.IdManager == m.Id).Name.StartsWith(value)).ToList(),
                6 => ourClubs.Where(c => managers.First(m => c.IdManager == m.Id).Patronymic.StartsWith(value)).ToList(),
                7 => ourClubs.Where(c => managers.First(m => c.IdManager == m.Id).Phone.StartsWith(value)).ToList(),
                8 => ourClubs.Where(c => cities.First(city => c.IdCity == city.Id).City.StartsWith(value)).ToList(),
                _ => null
            };

            // если подтвердили удаление то выполняем
            // иначе ничего не делаем
            if (MessageBox.Show($"Вы точно хотите удалить {deletes.Count} записей?",
                                "Подтверждение",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return false;

            // удалить записи
            foreach (var item in deletes)
            {
                // удалить выбранную запись
                db.Entry(item).State = EntityState.Deleted;
                db.OurClubs.Remove(item);
            }

            // записать в базу данных
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return true;
    }


    // удаление группы записей EnemyClubs по полю 
    public static bool DeleteEnemyClubsByCol(int index, string value)
    {
        try
        {
            List<EnemyClubs> enemyClubs = ShowTableEnemyClubs();

            List<Countries> countries = ShowTableCountries();

            // найти все записи подходящие для удаления
            var deletes = index switch
            {
                0 => enemyClubs.Where(c => c.Opposing.StartsWith(value)).ToList(),
                1 => enemyClubs.Where(c => countries.First(coun => c.IdCountry == coun.Id).Country.StartsWith(value)).ToList(),
                2 => enemyClubs.Where(c => c.SurnameCoach.StartsWith(value)).ToList(),
                3 => enemyClubs.Where(c => c.NameCoach.StartsWith(value)).ToList(),
                4 => enemyClubs.Where(c => c.PatronymicCoach.StartsWith(value)).ToList(),
                _ => null
            };

            // если подтвердили удаление то выполняем
            // иначе ничего не делаем
            if (MessageBox.Show($"Вы точно хотите удалить {deletes.Count} записей?",
                                "Подтверждение",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return false;

            // удалить записи
            foreach (var item in deletes)
            {
                // удалить выбранную запись
                db.Entry(item).State = EntityState.Deleted;
                db.EnemyClubs.Remove(item);
            }

            // записать в базу данных
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return true;
    }


    // удаление группы записей Cities по полю 
    public static bool DeleteCitiesByCol(int index, string value)
    {
        try
        {
            List<Cities> cities = ShowTableCities();

            // найти все записи подходящие для удаления
            var deletes = cities.Where(c => c.City.StartsWith(value)).ToList();

            // если подтвердили удаление то выполняем
            // иначе ничего не делаем
            if (MessageBox.Show($"Вы точно хотите удалить {deletes.Count} записей?",
                                "Подтверждение",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return false;

            // удалить записи
            foreach (var item in deletes)
            {
                // удалить выбранную запись
                db.Entry(item).State = EntityState.Deleted;
                db.Cities.Remove(item);
            }

            // записать в базу данных
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return true;
    }


    // удаление группы записей Countries по полю 
    public static bool DeleteCountriesByCol(int index, string value)
    {
        try
        {
            List<Countries> countries = ShowTableCountries();

            // найти все записи подходящие для удаления
            var deletes = countries.Where(c => c.Country.StartsWith(value)).ToList();

            // если подтвердили удаление то выполняем
            // иначе ничего не делаем
            if (MessageBox.Show($"Вы точно хотите удалить {deletes.Count} записей?",
                                "Подтверждение",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return false;

            // удалить записи
            foreach (var item in deletes)
            {
                // удалить выбранную запись
                db.Entry(item).State = EntityState.Deleted;
                db.Countries.Remove(item);
            }

            // записать в базу данных
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return true;
    }


    // удаление группы записей GameLevels по полю 
    public static bool DeleteGameLevelsByCol(int index, string value)
    {
        try
        {
            List<GameLevels> levels = ShowTableLevels();

            // найти все записи подходящие для удаления
            var deletes = levels.Where(c => c.GameLevel.StartsWith(value)).ToList();

            // если подтвердили удаление то выполняем
            // иначе ничего не делаем
            if (MessageBox.Show($"Вы точно хотите удалить {deletes.Count} записей?",
                                "Подтверждение",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return false;

            // удалить записи
            foreach (var item in deletes)
            {
                // удалить выбранную запись
                db.Entry(item).State = EntityState.Deleted;
                db.GameLevels.Remove(item);
            }

            // записать в базу данных
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return true;
    }


    // удаление группы записей Positions по полю 
    public static bool DeletePositionsByCol(int index, string value)
    {
        try
        {
            List<Positions> positions = ShowTablePositions();

            // найти все записи подходящие для удаления
            var deletes = positions.Where(c => c.Position.StartsWith(value)).ToList();

            // если подтвердили удаление то выполняем
            // иначе ничего не делаем
            if (MessageBox.Show($"Вы точно хотите удалить {deletes.Count} записей?",
                                "Подтверждение",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return false;

            // удалить записи
            foreach (var item in deletes)
            {
                // удалить выбранную запись
                db.Entry(item).State = EntityState.Deleted;
                db.Positions.Remove(item);
            }

            // записать в базу данных
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return true;
    }


    // удаление группы записей Leages по полю 
    public static bool DeleteLeagesByCol(int index, string value)
    {
        try
        {
            List<Leages> leages = ShowTableLeages();

            // найти все записи подходящие для удаления
            var deletes = leages.Where(c => c.Leage.StartsWith(value)).ToList();

            // если подтвердили удаление то выполняем
            // иначе ничего не делаем
            if (MessageBox.Show($"Вы точно хотите удалить {deletes.Count} записей?",
                                "Подтверждение",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return false;

            // удалить записи
            foreach (var item in deletes)
            {
                // удалить выбранную запись
                db.Entry(item).State = EntityState.Deleted;
                db.Leages.Remove(item);
            }

            // записать в базу данных
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return true;
    }



    // удаление группы записей TrainingBasis по полю 
    public static bool DeleteTrainingBasesByCol(int index, string value)
    {
        try
        {
            List<TrainingBasis> bases = ShowTableTrainingBasis();

            // найти все записи подходящие для удаления
            var deletes = bases.Where(c => c.Base.StartsWith(value)).ToList();

            // если подтвердили удаление то выполняем
            // иначе ничего не делаем
            if (MessageBox.Show($"Вы точно хотите удалить {deletes.Count} записей?",
                                "Подтверждение",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return false;

            // удалить записи
            foreach (var item in deletes)
            {
                // удалить выбранную запись
                db.Entry(item).State = EntityState.Deleted;
                db.TrainingBases.Remove(item);
            }

            // записать в базу данных
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return true;
    }


    // удаление группы записей EnemyClubs по полю 
    public static bool DeleteManagersByCol(int index, string value)
    {
        try
        {
            List<Managers> managers = ShowTableManagers();

            // найти все записи подходящие для удаления
            var deletes = index switch
            {
                0 => managers.Where(c => c.Surname.StartsWith(value)).ToList(),
                1 => managers.Where(c => c.Name.StartsWith(value)).ToList(),
                2 => managers.Where(c => c.Patronymic.StartsWith(value)).ToList(),
                3 => managers.Where(c => c.Phone.StartsWith(value)).ToList(),
                _ => null
            };

            // если подтвердили удаление то выполняем
            // иначе ничего не делаем
            if (MessageBox.Show($"Вы точно хотите удалить {deletes.Count} записей?",
                                "Подтверждение",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return false;

            // удалить записи
            foreach (var item in deletes)
            {
                // удалить выбранную запись
                db.Entry(item).State = EntityState.Deleted;
                db.Managers.Remove(item);
            }

            // записать в базу данных
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return true;
    }


    #endregion    


    #region Find

    // поиск группы записей в GameIns по полю 
    public static List<GameIns> FindGameInsByCol(int index, string value)
    {
        List<GameIns> gameIns = ShowTableGameIns();

        List<Games> games = ShowTableGames();
        List<Gamers> gamers = ShowTableGamers();
        List<OurClubs> ourClubs = ShowTableOurClubs();
        List<EnemyClubs> enemyClubs = ShowTableEnemyClubs();

        // найти все подходящие записи
        return index switch
        {
            0 => gameIns.Where(g => gamers.First(gamer => g.IdGamer == gamer.Id).Surname.StartsWith(value)).ToList(),
            1 => gameIns.Where(g => gamers.First(gamer => g.IdGamer == gamer.Id).Name.StartsWith(value)).ToList(),
            2 => gameIns.Where(g => gamers.First(gamer => g.IdGamer == gamer.Id).Patronymic.StartsWith(value)).ToList(),
            3 => gameIns.Where(g => games.First(game => g.IdGame == game.Id).DateGame == DateOnly.Parse(value)).ToList(),
            4 => gameIns.Where(g => enemyClubs.First(club => games.First(game => g.IdGame == game.Id).IdEnemyClub == club.Id).Opposing.StartsWith(value)).ToList(),
            5 => gameIns.Where(g => ourClubs.First(club => games.First(game => g.IdGame == game.Id).IdOurClub == club.Id).Club.StartsWith(value)).ToList(),
            6 => gameIns.Where(g => g.Order == (value == "участвовал" ? true : false)).ToList(),
            7 => gameIns.Where(g => g.CountStart == int.Parse(value)).ToList(),
            8 => gameIns.Where(g => g.Salary == long.Parse(value)).ToList(),
            _ => null
        };
    }

    // поиск группы записей Games по полю 
    public static List<Games> FindGamesByCol(int index, string value)
    {
        List<Games> games = ShowTableGames();

        List<GameIns> gameIns = ShowTableGameIns();
        List<Countries> countries = ShowTableCountries();
        List<GameLevels> levels = ShowTableLevels();
        List<OurClubs> ourClubs = ShowTableOurClubs();
        List<EnemyClubs> enemyClubs = ShowTableEnemyClubs();

        // найти все подходящие записи
        return index switch
        {
            0 => games.Where(g => g.DateGame == DateOnly.Parse(value)).ToList(),
            1 => games.Where(g => countries.First(coun => g.IdCountry == coun.Id).Country.StartsWith(value)).ToList(),
            2 => games.Where(g => levels.First(l => g.IdLevel == l.Id).GameLevel.StartsWith(value)).ToList(),
            3 => games.Where(g => g.CountFinish == int.Parse(value)).ToList(),
            4 => games.Where(g => enemyClubs.First(c => g.IdEnemyClub == c.Id).Opposing.StartsWith(value)).ToList(),
            5 => games.Where(g => ourClubs.First(c => g.IdOurClub == c.Id).Club.StartsWith(value)).ToList(),
            _ => null
        };
    }

    // поиск группы записей Gamers по полю 
    public static List<Gamers> FindGamersByCol(int index, string value)
    {
        List<Gamers> gamers = ShowTableGamers();

        List<Positions> positions = ShowTablePositions();
        List<OurClubs> ourClubs = ShowTableOurClubs();

        // найти все подходящие записи
        return index switch
        {
            0 => gamers.Where(g => g.Surname.StartsWith(value)).ToList(),
            1 => gamers.Where(g => g.Name.StartsWith(value)).ToList(),
            2 => gamers.Where(g => g.Patronymic.StartsWith(value)).ToList(),
            3 => gamers.Where(g => positions.First(p => g.IdPosition == p.Id).Position.StartsWith(value)).ToList(),
            4 => gamers.Where(g => g.Birthday == DateOnly.Parse(value)).ToList(),
            5 => gamers.Where(g => ourClubs.First(c => g.IdClub == c.Id).Club.StartsWith(value)).ToList(),
            6 => gamers.Where(g => g.YearFact == int.Parse(value)).ToList(),
            7 => gamers.Where(g => g.Comments.StartsWith(value)).ToList(),
            8 => gamers.Where(g => g.Cost == int.Parse(value)).ToList(),
            _ => null
        };
    }

    // поиск группы записей OurClubs по полю 
    public static List<OurClubs> FindOurClubsByCol(int index, string value)
    {
        List<OurClubs> ourClubs = ShowTableOurClubs();

        List<TrainingBasis> bases = ShowTableTrainingBasis();
        List<Managers> managers = ShowTableManagers();
        List<Cities> cities = ShowTableCities();
        List<Leages> leages = ShowTableLeages();

        // найти все подходящие записи
        return index switch
        {
            0 => ourClubs.Where(c => c.Club.StartsWith(value)).ToList(),
            1 => ourClubs.Where(c => bases.First(b => c.IdBase == b.Id).Base.StartsWith(value)).ToList(),
            2 => ourClubs.Where(c => c.Year == int.Parse(value)).ToList(),
            3 => ourClubs.Where(c => leages.First(l => c.IdLeage == l.Id).Leage.StartsWith(value)).ToList(),
            4 => ourClubs.Where(c => managers.First(m => c.IdManager == m.Id).Surname.StartsWith(value)).ToList(),
            5 => ourClubs.Where(c => managers.First(m => c.IdManager == m.Id).Name.StartsWith(value)).ToList(),
            6 => ourClubs.Where(c => managers.First(m => c.IdManager == m.Id).Patronymic.StartsWith(value)).ToList(),
            7 => ourClubs.Where(c => managers.First(m => c.IdManager == m.Id).Phone.StartsWith(value)).ToList(),
            8 => ourClubs.Where(c => cities.First(city => c.IdCity == city.Id).City.StartsWith(value)).ToList(),
            _ => null
        };
    }


    // поиск группы записей EnemyClubs по полю
    public static List<EnemyClubs> FindEnemyClubsByCol(int index, string value)
    {
        List<EnemyClubs> enemyClubs = ShowTableEnemyClubs();

        List<Countries> countries = ShowTableCountries();

        // найти все подходящие записи
        return index switch
        {
            0 => enemyClubs.Where(c => c.Opposing.StartsWith(value)).ToList(),
            1 => enemyClubs.Where(c => countries.First(coun => c.IdCountry == coun.Id).Country.StartsWith(value)).ToList(),
            2 => enemyClubs.Where(c => c.SurnameCoach.StartsWith(value)).ToList(),
            3 => enemyClubs.Where(c => c.NameCoach.StartsWith(value)).ToList(),
            4 => enemyClubs.Where(c => c.PatronymicCoach.StartsWith(value)).ToList(),
            _ => null
        };
    }


    // поиск группы записей Cities по полю 
    public static List<Cities> FindCitiesByCol(string value)
    {
        List<Cities> cities = ShowTableCities();

        // найти все подходящие записи
        return cities.Where(c => c.City.StartsWith(value)).ToList();
    }


    // поиск группы записей Countries по полю 
    public static List<Countries> FindCountriesByCol(string value)
    {
        List<Countries> countries = ShowTableCountries();

        // найти все записи подходящие для удаления
        return countries.Where(c => c.Country.StartsWith(value)).ToList();
    }


    // поиск группы записей GameLevels по полю 
    public static List<GameLevels> FindGameLevelsByCol(string value)
    {
        List<GameLevels> levels = ShowTableLevels();

        // найти все записи подходящие для удаления
        return levels.Where(c => c.GameLevel.StartsWith(value)).ToList();
    }


    // поиск группы записей Positions по полю 
    public static List<Positions> FindPositionsByCol(string value)
    {
        List<Positions> positions = ShowTablePositions();

        // найти все записи подходящие для удаления
        return positions.Where(c => c.Position.StartsWith(value)).ToList();
    }


    // поиск группы записей Leages по полю 
    public static List<Leages> FindLeagesByCol(string value)
    {
        List<Leages> leages = ShowTableLeages();

        // найти все записи подходящие для удаления
        return leages.Where(c => c.Leage.StartsWith(value)).ToList();
    }



    // поиск группы записей TrainingBasis по полю 
    public static List<TrainingBasis> FindTrainingBasesByCol(string value)
    {
        List<TrainingBasis> bases = ShowTableTrainingBasis();

        // найти все записи подходящие для удаления
        return bases.Where(c => c.Base.StartsWith(value)).ToList();
    }


    // поиск группы записей EnemyClubs по полю 
    public static List<Managers> FindManagersByCol(int index, string value)
    {
        List<Managers> managers = ShowTableManagers();

        // найти все записи подходящие для удаления
        return index switch
        {
            0 => managers.Where(c => c.Surname.StartsWith(value)).ToList(),
            1 => managers.Where(c => c.Name.StartsWith(value)).ToList(),
            2 => managers.Where(c => c.Patronymic.StartsWith(value)).ToList(),
            3 => managers.Where(c => c.Phone.StartsWith(value)).ToList(),
            _ => null
        };
    }


    #endregion

    // каскадное удаление игрока и его участий в играх
    public static bool CascadeDelete(int index)
    {
        try
        {
            List<GameIns> gameIns = ShowTableGameIns();
            List<Gamers> gamers = ShowTableGamers();

            // определение удаляемого игрока
            Gamers curGamer = gamers[index];

            List<Games> games = ShowTableGames();
            List<OurClubs> ourClubs = ShowTableOurClubs();
            List<EnemyClubs> enemyClubs = ShowTableEnemyClubs();

            // найти все записи подходящие для удаления
            var deletes = gameIns.Where(g => g.IdGamer == curGamer.Id)
                                 .ToList();

            // если подтвердили удаление то выполняем
            // иначе ничего не делаем
            if (MessageBox.Show($"Вы точно хотите каскадно удалить этого игрока?\nЭто приведёт к удалению {deletes.Count} записей(-и) его участий в играх",
                                "Подтверждение",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return false;

            // удалить записи
            foreach (var item in deletes)
            {
                // удалить выбранную запись
                db.Entry(item).State = EntityState.Deleted;
                db.GameIns.Remove(item);
            }

            // удалить игрока
            db.Entry(curGamer).State = EntityState.Deleted;
            db.Gamers.Remove(curGamer);

            // записать в базу данных
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return true;
    }
    

    #region Простые_запросы
    // запрос: учёт игроков на определённых позициях 
    public static List<GamerWithoutIds> ExecGamersByPosition(string pos)
    {
        List<GamerWithoutIds> list = new List<GamerWithoutIds>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"GamersByPositionFunc\"('{pos}')");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new GamerWithoutIds
            {
                Id = reader.GetInt32(0),
                Surname = reader.GetString(1),
                Name = reader.GetString(2),
                Patronymic = reader.GetString(3),
                Position = reader.GetString(4),
                Birthday = DateOnly.FromDateTime(reader.GetDateTime(5)),
                YearFact = reader.GetInt32(6),
                Club = reader.GetString(7),
                Photo = (byte[])reader.GetValue(8),
                Сomments = reader.GetString(9),
                Cost = reader.GetInt64(10)
            });
        }
        return list.OrderBy(g => g.Id).ToList();
    }

    // запрос: игроки родившиеся в определённую дату
    public static List<GamerWithoutIds> ExecGamersByBirthday(string pos)
    {
        List<GamerWithoutIds> list = new List<GamerWithoutIds>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"GamersByBirthdayFunc\"('{pos}')");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new GamerWithoutIds
            {
                Id = reader.GetInt32(0),
                Surname = reader.GetString(1),
                Name = reader.GetString(2),
                Patronymic = reader.GetString(3),
                Position = reader.GetString(4),
                Birthday = DateOnly.FromDateTime(reader.GetDateTime(5)),
                YearFact = reader.GetInt32(6),
                Club = reader.GetString(7),
                Photo = (byte[])reader.GetValue(8),
                Сomments = reader.GetString(9),
                Cost = reader.GetInt64(10)
            });
        }
        return list.OrderBy(g => g.Id).ToList();
    }

    // запрос: игры проходящие в определённой стране
    public static List<ShowGame> ExecGamesByCountry(string country)
    {
        List<ShowGame> list = new List<ShowGame>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"GamesByCountryFunc\"('{country}')");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new ShowGame
            (
                reader.GetInt32(0),                           // Id  
                DateOnly.FromDateTime(reader.GetDateTime(1)), // DateGame
                reader.GetString(2),                          // Country
                reader.GetString(3),                          // Gamelevel
                reader.GetInt32(4),                           // CountFinish
                reader.GetString(5),                          // Opposing
                reader.GetString(6)                           // Club
            ));
        }
        return list.OrderBy(g => g.Id).ToList();
    }

    // запрос: игры определённой даты
    public static List<ShowGame> ExecGamesByDate(string date)
    {
        List<ShowGame> list = new List<ShowGame>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"GamesByDateFunc\"('{date}')");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new ShowGame
            (
                reader.GetInt32(0),                           // Id  
                DateOnly.FromDateTime(reader.GetDateTime(1)), // DateGame
                reader.GetString(2),                          // Country
                reader.GetString(3),                          // Gamelevel
                reader.GetInt32(4),                           // CountFinish
                reader.GetString(5),                          // Opposing
                reader.GetString(6)                           // Club
            ));
        }
        return list.OrderBy(g => g.Id).ToList();
    }

    // запрос: вывод игроков через представление 
    public static List<ShowGamer> ExecFullGamers()
    {
        List<ShowGamer> list = new List<ShowGamer>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"FullGamersView\"");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new ShowGamer
            (
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4),
                DateOnly.FromDateTime(reader.GetDateTime(5)),
                reader.GetString(7),
                reader.GetInt32(6),
                null!,
                reader.GetString(9),
                reader.GetInt64(10)
            ));
        }
        return list.OrderBy(g => g.Id).ToList();
    }

    // запрос: вывод игр через представление 
    public static List<ShowGame> ExecFullGames()
    {
        List<ShowGame> list = new List<ShowGame>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"FullGamesView\"");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new ShowGame
            (
                reader.GetInt32(0),                           // Id  
                DateOnly.FromDateTime(reader.GetDateTime(1)), // DateGame
                reader.GetString(2),                          // Country
                reader.GetString(3),                          // Gamelevel
                reader.GetInt32(4),                           // CountFinish
                reader.GetString(5),                          // Opposing
                reader.GetString(6)                           // Club
            ));
        }
        return list.OrderBy(g => g.Id).ToList();
    }

    // запрос: вывод наших клубов через представление 
    public static List<ShowOurClub> ExecFullOurClub()
    {
        List<ShowOurClub> list = new List<ShowOurClub>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"FullOurClubView\"");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new ShowOurClub
            (
                reader.GetInt32(0),  // Id  
                reader.GetString(1), // Club
                reader.GetString(2), // Base
                reader.GetInt32(3),  // Year
                reader.GetString(4), // League
                reader.GetString(5), // Surname
                reader.GetString(6), // Name
                reader.GetString(7), // Patronymic
                reader.GetString(8), // Phone
                reader.GetString(9)  // City
            ));
        }
        return list.OrderBy(g => g.Id).ToList();
    }

    // запрос: Страны, в которых не проводились игры 
    public static List<string> ExecNullCountryGames()
    {
        List<string> list = new List<string>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"NullCountryGamesView\"");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(reader.GetString(0));
        }
        return list;
    }


    // запрос: Игроки, не учувствовавшие в матчах
    public static List<GamersForNull> ExecNullGamerIns()
    {
        List<GamersForNull> list = new List<GamersForNull>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"NullGamerInsView\"");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new GamersForNull(
                    reader.GetString(0), // Surname
                    reader.GetString(1), // Name
                    reader.GetString(2), // Patronymic
                    reader.GetString(3), // Position
                    reader.GetString(4) // Club
                    ));
        }
        return list;
    }


    // запрос: Игроки, которые не участвовали в играх определённой даты
    public static List<GamersForNull> ExecGamersNotSelDate(string date)
    {
        List<GamersForNull> list = new List<GamersForNull>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"GamersNotSelDateFunc\"('{date}')");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new GamersForNull(
                    reader.GetString(0), // Surname
                    reader.GetString(1), // Name
                    reader.GetString(2), // Patronymic
                    reader.GetString(3), // Position
                    reader.GetString(4) // Club
                    ));
        }
        return list;
    }
    #endregion


    #region Итоговые_запросы

    static List<CountryCount> list = new List<CountryCount>();

    // запрос: Количество проводимых игр всего и в странах
    public static List<CountryCount> ExecGamesCountryCount()
    {
        bool flag = true;

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"GamesCountryCountView\"");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            string country = "";

            if (flag)
            {
                country = "Всего";
                flag = false;
            }
            else
                country = reader.GetString(0);

            list.Add(new CountryCount {
                Country = country,
                Amount = reader.GetInt32(1)
            });
        }
        return list;
    }

    // вывод результата запроса в таблицу
    public static void PrintExcelFile()
    { 
        // если список пуст выполнить фунцию
        if (list.Count == 0)
            ExecGamesCountryCount();

        // создаём файл
        Application app = new Application();

        // добавляем в файл книгу
        app.Workbooks.Add();

        // добавить в книгу лист
        Worksheet worksheet = (Worksheet)app.ActiveSheet;

        // вывод названий столбцов
        worksheet.Cells[1, 1] = "Название страны";
        worksheet.Cells[1, 2] = "Количество игр";

        // вывод информации в таблицу
        for (int i = 0; i < list.Count; i++)
        {
            worksheet.Cells[i + 2, 1] = list[i].Country;
            worksheet.Cells[i + 2, 2] = list[i].Amount.ToString();
        }

        // отобразить результат
        app.Visible = true;
    }

    // вернуть из списка пары ключ значение
    public static List<KeyValuePair<string, int>> GetKeyValueList()
    {
        // если список пуст выполнить фунцию
        if (list.Count == 0)
            ExecGamesCountryCount();

        var res = new List<KeyValuePair<string, int>>();

        for(int i = 1; i < list.Count; i++)
            res.Add(new KeyValuePair<string, int>(list[i].Country, list[i].Amount));

        return res;
    }


    // запрос: Количество игроков в каждой команде со стоимостью контракта больше указанной
    public static List<GroupQuery> ExecClubGamersCost(string value)
    {
        // из строки делаем число
        long.TryParse(value, out long data);

        List<GroupQuery> list = new List<GroupQuery>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"ClubGamersCostUndFunc\"({data})");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new GroupQuery {
                Name = reader.GetString(0), // Name
                Amount = reader.GetInt64(1) // Amount
            });
        }
        return list;
    }

    // запрос: Команды с средней стоимостью контракта игроков больше указанной 
    public static List<GroupQuery> ExecClubGamersAvgCost(string value)
    {
        // из строки делаем число
        long.TryParse(value, out long data);

        List<GroupQuery> list = new List<GroupQuery>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"ClubGamersAvgCostFunc\"({data})");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new GroupQuery {
                Name = reader.GetString(0), // Name
                Amount = reader.GetInt64(1) // Amount
            });
        }
        return list;
    }

    // запрос: Команды, где суммарная стоимость контракта игроков на определённой позиции больше указанной
    public static List<GroupQuery> ExecClubsGamerPosSumCost(string pos, string cost)
    {
        // из строки делаем число
        long.TryParse(cost, out long data);

        List<GroupQuery> list = new List<GroupQuery>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"ClubsGamerPosSumCostFunc\"('{pos}', {data})");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new GroupQuery {
                Name = reader.GetString(0), // Name
                Amount = reader.GetInt64(1) // Amount
            });
        }
        return list;
    }

    // запрос: Количество игроков с минимальной стоимостью контракта в каждой команде
    public static List<GroupQuery> ExecClubGamersMaxCostCount()
    {
        List<GroupQuery> list = new List<GroupQuery>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"ClubGamersMaxCostCountView\"");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new GroupQuery {
                Name = reader.GetString(0), // Name
                Amount = reader.GetInt64(1) // Amount
            });
        }
        return list;
    }

    // запрос: Клубы, находящиеся в городе Москва
    public static List<string> ExecMoscowClubs()
    {
        List<string> list = new List<string>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"MoscowClubsView\"");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(reader.GetString(0));
        }
        return list;
    }

    // запрос: Клубы, находящиеся не в городе Москва
    public static List<string> ExecNotMoscowClubs()
    {
        List<string> list = new List<string>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"NotMoscowClubsView\"");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(reader.GetString(0));
        }
        return list;
    }

    // запрос case: Участие игроков в матчах (case)
    public static List<ForCaseQuery> ExecCaseGameIns()
    {
        List<ForCaseQuery> list = new List<ForCaseQuery>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"CaseGameInsView\"");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new ForCaseQuery
            {
                Surname = reader.GetString(0),
                Name = reader.GetString(1), 
                Patronymic = reader.GetString(2), 
                DateGame = DateOnly.FromDateTime(reader.GetDateTime(3)),
                OurClub = reader.GetString(4),
                EnemyClub = reader.GetString(5),
                Order = reader.GetString(6)
            });
        }
        return list;
    }

    // запрос без идекса: количество провведённых игр по странам за последний год
    public static List<GroupQuery> ExecWithoutIndex()
    {
        List<GroupQuery> list = new List<GroupQuery>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"WithoutIndexView\"");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new GroupQuery
            {
                Name = reader.GetString(0), // Name
                Amount = reader.GetInt64(1) // Amount
            });
        }
        return list;
    }

    // запрос с идексом: количество игроков в клубах со стоимостью контракта больше 50 000 000
    public static List<GroupQuery> ExecWithIndex()
    {
        List<GroupQuery> list = new List<GroupQuery>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"WithIndexQueryView\"");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new GroupQuery
            {
                Name = reader.GetString(0), // Name
                Amount = reader.GetInt64(1) // Amount
            });
        }
        return list;
    }

    // запрос по маске: игроки фамилия которых начинается на букву М
    public static List<GroupQuery> ExecMask()
    {
        List<GroupQuery> list = new List<GroupQuery>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"MaskQueryView\"");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new GroupQuery
            {
                Name = reader.GetString(0), // Name
                Amount = reader.GetInt64(1) // Amount
            });
        }
        return list;
    }

    // запрос: Определить 3 лучших футболиста каждой команды (по количеству забитых голов) и 3 лучшие команды.
    public static List<ForFirstCursQuery> ExecFirstCurs()
    {
        List<ForFirstCursQuery> list = new List<ForFirstCursQuery>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"FirstCursQueryView\"");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new ForFirstCursQuery
            {
                Surname = reader.GetString(0),
                Name = reader.GetString(1),
                Patronymic = reader.GetString(2),
                OurClub = reader.GetString(3),
                SumGols = reader.GetInt32(4)
            });
        }
        return list;
    }

    // запрос: Определить среднее количество забитых и пропущенных мячей каждым игроком по каждой
    // нашей команде и по каждой команде в целом
    public static List<ForSecondCursQuery> ExecSecondCurs()
    {
        List<ForSecondCursQuery> list = new List<ForSecondCursQuery>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"SecondCursQueryView\"");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new ForSecondCursQuery
            {
                Surname = reader.GetString(0),
                Name = reader.GetString(1),
                Patronymic = reader.GetString(2),
                OurClub = reader.GetString(3),
                AvgGols = reader.GetDouble(4),
                AvgMiss = reader.GetDouble(5)
            });
        }
        return list;
    }

    // запрос: Определить количество игр и финансирование выбранного клуба за указанный год
    public static List<ForThirdCursQuery> ExecThirdCurs(string club, string year)
    {
        // из строки делаем число
        int.TryParse(year, out int data);

        List<ForThirdCursQuery> list = new List<ForThirdCursQuery>();

        NpgsqlDataReader reader = DbExecSqlCommand($"SELECT * FROM \"ThirdCursQueryFunc\"('{club}', {data})");

        // Если данные получены (есть строки в полученном ответе сервера)
        while (reader.Read())
        {
            list.Add(new ForThirdCursQuery
            {
                Club = reader.GetString(0),
                CountGames = reader.GetInt32(1),
                SumSalary = reader.GetInt64(2) 
            });
        }
        return list;
    }

    #endregion

    // подключиться к базе и выдать результат запроса
    private static NpgsqlDataReader DbExecSqlCommand(string query)
    {
        // подключение к БД
        NpgsqlConnection connection = new NpgsqlConnection(connectionString);
        connection.Open(); // подключение к серверу, блокирующий вызов

        // создание команды (запрос SQL), привязка к соединению
        NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
        cmd.CommandType = CommandType.Text;

        // задать соединение с БД
        cmd.Connection = connection;

        // выполнение запроса, ссылка на выбранные данные - reader
        return cmd.ExecuteReader();
    }
}

public class GamerWithoutIds
{
    public int Id { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronymic { get; set; }
    public string Position { get; set; }
    public DateOnly Birthday { get; set; }
    public string Club { get; set; }
    public int YearFact { get; set; }
    public byte[] Photo { get; set; }
    public string Сomments { get; set; }
    public long Cost { get; set; }
}

public class CountryCount
{
    public string Country { get; set; }

    public int Amount { get; set; }
}

public class GroupQuery
{
    public string Name { get; set; }

    public long Amount { get; set; }
}

public class ForCaseQuery
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronymic { get; set; }
    public DateOnly DateGame { get; set; }
    public string OurClub { get; set; }
    public string EnemyClub { get; set; }
    public string Order { get; set; }
}

public class ForFirstCursQuery
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronymic { get; set; }
    public string OurClub { get; set; }
    public int SumGols { get; set; }
}

public class ForSecondCursQuery
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronymic { get; set; }
    public string OurClub { get; set; }
    public double AvgGols { get; set; }
    public double AvgMiss { get; set; }
}

public class ForThirdCursQuery
{
    public string Club { get; set; }

    public int CountGames { get; set; }

    public long SumSalary { get; set; }
}