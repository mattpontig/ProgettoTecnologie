-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Creato il: Gen 16, 2023 alle 17:57
-- Versione del server: 10.4.27-MariaDB
-- Versione PHP: 8.2.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `db_telegram`
--

-- --------------------------------------------------------

--
-- Struttura della tabella `chat`
--

CREATE TABLE `chat` (
  `idChat` int(11) NOT NULL,
  `gruppo` tinyint(1) NOT NULL,
  `titolo` varchar(25) DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dump dei dati per la tabella `chat`
--

INSERT INTO `chat` (`idChat`, `gruppo`, `titolo`) VALUES
(1, 0, ''),
(2, 1, 'primoGruppo'),
(3, 0, ''),
(7, 1, 'ProvaCreaGruppo');

-- --------------------------------------------------------

--
-- Struttura della tabella `login`
--

CREATE TABLE `login` (
  `id` int(11) NOT NULL,
  `user` varchar(25) NOT NULL,
  `pass` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dump dei dati per la tabella `login`
--

INSERT INTO `login` (`id`, `user`, `pass`) VALUES
(4, 'pippo', '0c88028bf3aa6a6a143ed846f2be1ea4'),
(5, 'prova', '189bbbb00c5f1fb7fba9ad9285f193d1'),
(6, 'prova2', '280093f2cfe260a00ee1bb06f96584de'),
(7, 'test', '098f6bcd4621d373cade4e832627b4f6');

-- --------------------------------------------------------

--
-- Struttura della tabella `messaggichat`
--

CREATE TABLE `messaggichat` (
  `idMex` int(11) NOT NULL,
  `messaggio` varchar(200) NOT NULL,
  `idChat` int(11) NOT NULL,
  `idMittente` int(11) NOT NULL,
  `file` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dump dei dati per la tabella `messaggichat`
--

INSERT INTO `messaggichat` (`idMex`, `messaggio`, `idChat`, `idMittente`, `file`) VALUES
(1, 'ciao', 1, 4, 0),
(2, 'ciao pippo!', 1, 5, 0),
(3, 'primo messaggio prova per gruppo', 2, 6, 0),
(4, 'prova send', 3, 4, 0),
(16, 'mmm', 3, 6, 0),
(17, 'funziona?', 3, 4, 0),
(18, 'penso di si', 3, 6, 0),
(19, 'prova nuovo send', 1, 4, 0),
(20, 'forse', 1, 5, 0),
(21, 'mmm', 1, 4, 0),
(22, 'riciao', 1, 4, 0),
(23, 'ei', 3, 4, 0),
(24, 'mm', 2, 4, 0),
(25, 'see', 3, 4, 0),
(26, 'boh', 3, 4, 0),
(27, 'bug', 2, 4, 0),
(28, 'rompo tutto', 3, 6, 0),
(29, ' non letto', 1, 4, 0),
(30, 'nuovo g', 7, 7, 0),
(31, 'ciao', 7, 4, 0),
(32, 'triplo', 2, 5, 0),
(33, 'ei', 2, 6, 0),
(34, 'come va', 3, 6, 0),
(35, 'ciao', 2, 4, 0),
(36, 'ciao', 1, 4, 0),
(37, 'come va', 3, 6, 0),
(38, 'ciao', 1, 4, 0);

-- --------------------------------------------------------

--
-- Struttura della tabella `utentichat`
--

CREATE TABLE `utentichat` (
  `idUtente` int(11) NOT NULL,
  `idChat` int(11) NOT NULL,
  `mesNonLetti` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dump dei dati per la tabella `utentichat`
--

INSERT INTO `utentichat` (`idUtente`, `idChat`, `mesNonLetti`) VALUES
(4, 1, 0),
(4, 2, 0),
(4, 3, 0),
(4, 7, 0),
(5, 1, 0),
(5, 2, 0),
(5, 7, 0),
(6, 2, 0),
(6, 3, 0),
(7, 7, 0);

--
-- Indici per le tabelle scaricate
--

--
-- Indici per le tabelle `chat`
--
ALTER TABLE `chat`
  ADD PRIMARY KEY (`idChat`);

--
-- Indici per le tabelle `login`
--
ALTER TABLE `login`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `messaggichat`
--
ALTER TABLE `messaggichat`
  ADD PRIMARY KEY (`idMex`) USING BTREE,
  ADD KEY `fkUtenti` (`idMittente`),
  ADD KEY `fkChat` (`idChat`);

--
-- Indici per le tabelle `utentichat`
--
ALTER TABLE `utentichat`
  ADD PRIMARY KEY (`idUtente`,`idChat`),
  ADD KEY `FK1` (`idChat`);

--
-- AUTO_INCREMENT per le tabelle scaricate
--

--
-- AUTO_INCREMENT per la tabella `chat`
--
ALTER TABLE `chat`
  MODIFY `idChat` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT per la tabella `login`
--
ALTER TABLE `login`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT per la tabella `messaggichat`
--
ALTER TABLE `messaggichat`
  MODIFY `idMex` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=39;

--
-- Limiti per le tabelle scaricate
--

--
-- Limiti per la tabella `messaggichat`
--
ALTER TABLE `messaggichat`
  ADD CONSTRAINT `fkChat` FOREIGN KEY (`idChat`) REFERENCES `chat` (`idChat`),
  ADD CONSTRAINT `fkUtenti` FOREIGN KEY (`idMittente`) REFERENCES `login` (`id`);

--
-- Limiti per la tabella `utentichat`
--
ALTER TABLE `utentichat`
  ADD CONSTRAINT `FK1` FOREIGN KEY (`idChat`) REFERENCES `chat` (`idChat`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK2` FOREIGN KEY (`idUtente`) REFERENCES `login` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
