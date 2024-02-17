-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1
-- Généré le : sam. 17 fév. 2024 à 14:08
-- Version du serveur : 10.4.32-MariaDB
-- Version de PHP : 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `planningsup`
--

-- --------------------------------------------------------

--
-- Structure de la table `coursset`
--

CREATE TABLE `coursset` (
  `Id` int(11) NOT NULL,
  `NiveauId` int(11) NOT NULL,
  `EnseignantId` int(11) NOT NULL,
  `Jour` varchar(255) NOT NULL,
  `Heure_debut` time NOT NULL,
  `Heure_fin` time NOT NULL,
  `UE_Id` int(11) NOT NULL,
  `Enseignant_Matricule` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déclencheurs `coursset`
--
DELIMITER $$
CREATE TRIGGER `tr_cours_delete` AFTER DELETE ON `coursset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Suppression dans la table CoursSet', 'CoursSet', OLD.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_cours_insert` AFTER INSERT ON `coursset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Insertion dans la table CoursSet', 'CoursSet', NEW.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_cours_update` AFTER UPDATE ON `coursset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Mise à jour dans la table CoursSet', 'CoursSet', NEW.Id);
    END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `courssettemp`
--

CREATE TABLE `courssettemp` (
  `Id` int(11) NOT NULL,
  `NiveauId` int(11) NOT NULL,
  `EnseignantId` int(11) NOT NULL,
  `Jour` varchar(255) NOT NULL,
  `Heure_debut` time NOT NULL,
  `Heure_fin` time NOT NULL,
  `valide` tinyint(1) DEFAULT NULL,
  `UE_Id` int(11) NOT NULL,
  `Enseignant_Matricule` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `departementset`
--

CREATE TABLE `departementset` (
  `Id` int(11) NOT NULL,
  `FaculteId` int(11) NOT NULL,
  `Nom` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déclencheurs `departementset`
--
DELIMITER $$
CREATE TRIGGER `tr_departement_delete` AFTER DELETE ON `departementset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Suppression dans la table DepartementSet', 'DepartementSet', OLD.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_departement_insert` AFTER INSERT ON `departementset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Insertion dans la table DepartementSet', 'DepartementSet', NEW.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_departement_update` AFTER UPDATE ON `departementset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Mise à jour dans la table DepartementSet', 'DepartementSet', NEW.Id);
    END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `faculteset`
--

CREATE TABLE `faculteset` (
  `Id` int(11) NOT NULL,
  `Nom` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déclencheurs `faculteset`
--
DELIMITER $$
CREATE TRIGGER `tr_faculte_delete` AFTER DELETE ON `faculteset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Suppression dans la table FaculteSet', 'FaculteSet', OLD.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_faculte_insert` AFTER INSERT ON `faculteset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Insertion dans la table FaculteSet', 'FaculteSet', NEW.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_faculte_update` AFTER UPDATE ON `faculteset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Mise à jour dans la table FaculteSet', 'FaculteSet', NEW.Id);
    END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `filiereset`
--

CREATE TABLE `filiereset` (
  `Id` int(11) NOT NULL,
  `DepartementId` int(11) NOT NULL,
  `Nom` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déclencheurs `filiereset`
--
DELIMITER $$
CREATE TRIGGER `tr_filiere_delete` AFTER DELETE ON `filiereset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Suppression dans la table FiliereSet', 'FiliereSet', OLD.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_filiere_insert` AFTER INSERT ON `filiereset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Insertion dans la table FiliereSet', 'FiliereSet', NEW.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_filiere_update` AFTER UPDATE ON `filiereset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Mise à jour dans la table FiliereSet', 'FiliereSet', NEW.Id);
    END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `journal`
--

CREATE TABLE `journal` (
  `Id` int(11) NOT NULL,
  `Action` varchar(255) NOT NULL,
  `Table_concernee` varchar(50) NOT NULL,
  `Action_sur_id` int(11) DEFAULT NULL,
  `Timestamp` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `journal_user`
--

CREATE TABLE `journal_user` (
  `Id` int(11) NOT NULL,
  `Action` varchar(255) NOT NULL,
  `Table_concernee` varchar(50) NOT NULL,
  `Action_sur_id` varchar(50) DEFAULT NULL,
  `Timestamp` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `niveauset`
--

CREATE TABLE `niveauset` (
  `Id` int(11) NOT NULL,
  `FiliereId` int(11) NOT NULL,
  `Libelle` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déclencheurs `niveauset`
--
DELIMITER $$
CREATE TRIGGER `tr_niveau_delete` AFTER DELETE ON `niveauset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Suppression dans la table NiveauSet', 'NiveauSet', OLD.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_niveau_insert` AFTER INSERT ON `niveauset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Insertion dans la table NiveauSet', 'NiveauSet', NEW.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_niveau_update` AFTER UPDATE ON `niveauset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Mise à jour dans la table NiveauSet', 'NiveauSet', NEW.Id);
    END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `ueset`
--

CREATE TABLE `ueset` (
  `Id` int(11) NOT NULL,
  `Code` varchar(50) NOT NULL,
  `Libelle` varchar(255) NOT NULL,
  `semestre` int(11) NOT NULL,
  `NiveauId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déclencheurs `ueset`
--
DELIMITER $$
CREATE TRIGGER `tr_ue_delete` AFTER DELETE ON `ueset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Suppression dans la table UESet', 'UESet', OLD.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_ue_insert` AFTER INSERT ON `ueset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Insertion dans la table UESet', 'UESet', NEW.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_ue_update` AFTER UPDATE ON `ueset` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Mise à jour dans la table UESet', 'UESet', NEW.Id);
    END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `utilisateurset`
--

CREATE TABLE `utilisateurset` (
  `Matricule` varchar(50) NOT NULL,
  `Nom` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `MotDePasse` varchar(255) NOT NULL,
  `Role` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déclencheurs `utilisateurset`
--
DELIMITER $$
CREATE TRIGGER `tr_utilisateur_delete` AFTER DELETE ON `utilisateurset` FOR EACH ROW BEGIN
        INSERT INTO `journal_user` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Suppression dans la table UtilisateurSet', 'UtilisateurSet', OLD.Matricule);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_utilisateur_insert` AFTER INSERT ON `utilisateurset` FOR EACH ROW BEGIN
        INSERT INTO `journal_user` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Insertion dans la table UtilisateurSet', 'UtilisateurSet', NEW.Matricule);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_utilisateur_update` AFTER UPDATE ON `utilisateurset` FOR EACH ROW BEGIN
        INSERT INTO `journal_user` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Mise à jour dans la table UtilisateurSet', 'UtilisateurSet', NEW.Matricule);
    END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `utilisateurset_administrateur`
--

CREATE TABLE `utilisateurset_administrateur` (
  `Id` int(11) NOT NULL,
  `Matricule` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déclencheurs `utilisateurset_administrateur`
--
DELIMITER $$
CREATE TRIGGER `tr_utilisateur_administrateur_delete` AFTER DELETE ON `utilisateurset_administrateur` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Suppression dans la table UtilisateurSet_Administrateur', 'UtilisateurSet_Administrateur', OLD.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_utilisateur_administrateur_insert` AFTER INSERT ON `utilisateurset_administrateur` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Insertion dans la table UtilisateurSet_Administrateur', 'UtilisateurSet_Administrateur', NEW.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_utilisateur_administrateur_update` AFTER UPDATE ON `utilisateurset_administrateur` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Mise à jour dans la table UtilisateurSet_Administrateur', 'UtilisateurSet_Administrateur', NEW.Id);
    END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `utilisateurset_enseignant`
--

CREATE TABLE `utilisateurset_enseignant` (
  `Id` int(11) NOT NULL,
  `Specialite` varchar(255) NOT NULL,
  `Matricule` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déclencheurs `utilisateurset_enseignant`
--
DELIMITER $$
CREATE TRIGGER `tr_utilisateur_enseignant_delete` AFTER DELETE ON `utilisateurset_enseignant` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Suppression dans la table UtilisateurSet_Enseignant', 'UtilisateurSet_Enseignant', OLD.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_utilisateur_enseignant_insert` AFTER INSERT ON `utilisateurset_enseignant` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Insertion dans la table UtilisateurSet_Enseignant', 'UtilisateurSet_Enseignant', NEW.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_utilisateur_enseignant_update` AFTER UPDATE ON `utilisateurset_enseignant` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Mise à jour dans la table UtilisateurSet_Enseignant', 'UtilisateurSet_Enseignant', NEW.Id);
    END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `utilisateurset_etudiant`
--

CREATE TABLE `utilisateurset_etudiant` (
  `Id` int(11) NOT NULL,
  `FiliereId` int(11) NOT NULL,
  `NiveauId` int(11) NOT NULL,
  `Matricule` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déclencheurs `utilisateurset_etudiant`
--
DELIMITER $$
CREATE TRIGGER `tr_utilisateur_etudiant_delete` AFTER DELETE ON `utilisateurset_etudiant` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Suppression dans la table UtilisateurSet_Etudiant', 'UtilisateurSet_Etudiant', OLD.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_utilisateur_etudiant_insert` AFTER INSERT ON `utilisateurset_etudiant` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Insertion dans la table UtilisateurSet_Etudiant', 'UtilisateurSet_Etudiant', NEW.Id);
    END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `tr_utilisateur_etudiant_update` AFTER UPDATE ON `utilisateurset_etudiant` FOR EACH ROW BEGIN
        INSERT INTO `Journal` (`Action`, `Table_concernee`, `Action_sur_id`) VALUES ('Mise à jour dans la table UtilisateurSet_Etudiant', 'UtilisateurSet_Etudiant', NEW.Id);
    END
$$
DELIMITER ;

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `coursset`
--
ALTER TABLE `coursset`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `NiveauId` (`NiveauId`),
  ADD KEY `UE_Id` (`UE_Id`),
  ADD KEY `Enseignant_Matricule` (`Enseignant_Matricule`);

--
-- Index pour la table `courssettemp`
--
ALTER TABLE `courssettemp`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `NiveauId` (`NiveauId`),
  ADD KEY `UE_Id` (`UE_Id`),
  ADD KEY `Enseignant_Matricule` (`Enseignant_Matricule`);

--
-- Index pour la table `departementset`
--
ALTER TABLE `departementset`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `FaculteId` (`FaculteId`);

--
-- Index pour la table `faculteset`
--
ALTER TABLE `faculteset`
  ADD PRIMARY KEY (`Id`);

--
-- Index pour la table `filiereset`
--
ALTER TABLE `filiereset`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `DepartementId` (`DepartementId`);

--
-- Index pour la table `journal`
--
ALTER TABLE `journal`
  ADD PRIMARY KEY (`Id`);

--
-- Index pour la table `journal_user`
--
ALTER TABLE `journal_user`
  ADD PRIMARY KEY (`Id`);

--
-- Index pour la table `niveauset`
--
ALTER TABLE `niveauset`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `FiliereId` (`FiliereId`);

--
-- Index pour la table `ueset`
--
ALTER TABLE `ueset`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `NiveauId` (`NiveauId`);

--
-- Index pour la table `utilisateurset`
--
ALTER TABLE `utilisateurset`
  ADD PRIMARY KEY (`Matricule`);

--
-- Index pour la table `utilisateurset_administrateur`
--
ALTER TABLE `utilisateurset_administrateur`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `Matricule` (`Matricule`);

--
-- Index pour la table `utilisateurset_enseignant`
--
ALTER TABLE `utilisateurset_enseignant`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `Matricule` (`Matricule`);

--
-- Index pour la table `utilisateurset_etudiant`
--
ALTER TABLE `utilisateurset_etudiant`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `FiliereId` (`FiliereId`),
  ADD KEY `NiveauId` (`NiveauId`),
  ADD KEY `Matricule` (`Matricule`);

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `coursset`
--
ALTER TABLE `coursset`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `courssettemp`
--
ALTER TABLE `courssettemp`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `departementset`
--
ALTER TABLE `departementset`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `faculteset`
--
ALTER TABLE `faculteset`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `filiereset`
--
ALTER TABLE `filiereset`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `journal`
--
ALTER TABLE `journal`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `journal_user`
--
ALTER TABLE `journal_user`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `niveauset`
--
ALTER TABLE `niveauset`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `ueset`
--
ALTER TABLE `ueset`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `utilisateurset_administrateur`
--
ALTER TABLE `utilisateurset_administrateur`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `utilisateurset_enseignant`
--
ALTER TABLE `utilisateurset_enseignant`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `utilisateurset_etudiant`
--
ALTER TABLE `utilisateurset_etudiant`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `coursset`
--
ALTER TABLE `coursset`
  ADD CONSTRAINT `coursset_ibfk_1` FOREIGN KEY (`NiveauId`) REFERENCES `niveauset` (`Id`),
  ADD CONSTRAINT `coursset_ibfk_2` FOREIGN KEY (`UE_Id`) REFERENCES `ueset` (`Id`),
  ADD CONSTRAINT `coursset_ibfk_3` FOREIGN KEY (`Enseignant_Matricule`) REFERENCES `utilisateurset` (`Matricule`);

--
-- Contraintes pour la table `courssettemp`
--
ALTER TABLE `courssettemp`
  ADD CONSTRAINT `courssettemp_ibfk_1` FOREIGN KEY (`NiveauId`) REFERENCES `niveauset` (`Id`),
  ADD CONSTRAINT `courssettemp_ibfk_2` FOREIGN KEY (`UE_Id`) REFERENCES `ueset` (`Id`),
  ADD CONSTRAINT `courssettemp_ibfk_3` FOREIGN KEY (`Enseignant_Matricule`) REFERENCES `utilisateurset` (`Matricule`);

--
-- Contraintes pour la table `departementset`
--
ALTER TABLE `departementset`
  ADD CONSTRAINT `departementset_ibfk_1` FOREIGN KEY (`FaculteId`) REFERENCES `faculteset` (`Id`);

--
-- Contraintes pour la table `filiereset`
--
ALTER TABLE `filiereset`
  ADD CONSTRAINT `filiereset_ibfk_1` FOREIGN KEY (`DepartementId`) REFERENCES `departementset` (`Id`);

--
-- Contraintes pour la table `niveauset`
--
ALTER TABLE `niveauset`
  ADD CONSTRAINT `niveauset_ibfk_1` FOREIGN KEY (`FiliereId`) REFERENCES `filiereset` (`Id`);

--
-- Contraintes pour la table `ueset`
--
ALTER TABLE `ueset`
  ADD CONSTRAINT `ueset_ibfk_1` FOREIGN KEY (`NiveauId`) REFERENCES `niveauset` (`Id`);

--
-- Contraintes pour la table `utilisateurset_administrateur`
--
ALTER TABLE `utilisateurset_administrateur`
  ADD CONSTRAINT `utilisateurset_administrateur_ibfk_1` FOREIGN KEY (`Matricule`) REFERENCES `utilisateurset` (`Matricule`);

--
-- Contraintes pour la table `utilisateurset_enseignant`
--
ALTER TABLE `utilisateurset_enseignant`
  ADD CONSTRAINT `utilisateurset_enseignant_ibfk_1` FOREIGN KEY (`Matricule`) REFERENCES `utilisateurset` (`Matricule`);

--
-- Contraintes pour la table `utilisateurset_etudiant`
--
ALTER TABLE `utilisateurset_etudiant`
  ADD CONSTRAINT `utilisateurset_etudiant_ibfk_1` FOREIGN KEY (`FiliereId`) REFERENCES `filiereset` (`Id`),
  ADD CONSTRAINT `utilisateurset_etudiant_ibfk_2` FOREIGN KEY (`NiveauId`) REFERENCES `niveauset` (`Id`),
  ADD CONSTRAINT `utilisateurset_etudiant_ibfk_3` FOREIGN KEY (`Matricule`) REFERENCES `utilisateurset` (`Matricule`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
