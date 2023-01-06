-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Creato il: Gen 06, 2023 alle 23:39
-- Versione del server: 10.4.25-MariaDB
-- Versione PHP: 8.1.10

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
  `titolo` varchar(25) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dump dei dati per la tabella `chat`
--

INSERT INTO `chat` (`idChat`, `gruppo`, `titolo`) VALUES
(1, 0, ''),
(2, 1, 'primo gruppo');

-- --------------------------------------------------------

--
-- Struttura della tabella `login`
--

CREATE TABLE `login` (
  `id` int(11) NOT NULL,
  `user` varchar(25) NOT NULL,
  `pass` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dump dei dati per la tabella `login`
--

INSERT INTO `login` (`id`, `user`, `pass`) VALUES
(4, 'pippo', '0c88028bf3aa6a6a143ed846f2be1ea4'),
(5, 'prova', '189bbbb00c5f1fb7fba9ad9285f193d1'),
(6, 'prova2', '280093f2cfe260a00ee1bb06f96584de');

-- --------------------------------------------------------

--
-- Struttura della tabella `messaggichat`
--

CREATE TABLE `messaggichat` (
  `idMex` int(11) NOT NULL,
  `messaggio` varchar(200) NOT NULL,
  `idChat` int(11) NOT NULL,
  `idMittente` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dump dei dati per la tabella `messaggichat`
--

INSERT INTO `messaggichat` (`idMex`, `messaggio`, `idChat`, `idMittente`) VALUES
(1, 'ciao', 1, 4),
(2, 'ciao pippo!', 1, 5),
(3, 'primo messaggio prova per gruppo', 2, 6);

-- --------------------------------------------------------

--
-- Struttura della tabella `utentichat`
--

CREATE TABLE `utentichat` (
  `idUtente` int(11) NOT NULL,
  `idChat` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dump dei dati per la tabella `utentichat`
--

INSERT INTO `utentichat` (`idUtente`, `idChat`) VALUES
(4, 1),
(4, 2),
(5, 1),
(5, 2),
(6, 2);

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
  MODIFY `idChat` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT per la tabella `login`
--
ALTER TABLE `login`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT per la tabella `messaggichat`
--
ALTER TABLE `messaggichat`
  MODIFY `idMex` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

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
