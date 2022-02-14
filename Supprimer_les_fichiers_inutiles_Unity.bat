@echo off
for /f "tokens=2 delims=:." %%x in ('chcp') do set cp=%%x
chcp 1252>nul

echo **************************************************************************
echo   TIM CSTJ - OUTIL DE R�DUCTION DES PROJETS UNITY.
echo   Ce script permet d'all�ger le poids d'un projet.
echo   Seuls des fichiers que Unity peut reconstruire seront supprim�s.
echo   Remarque: Unity doit �tre ferm� pour que le script soit fonctionnel.
echo   Le contenu du dossier suivant va �tre nettoy�:
echo   %cd%
echo **************************************************************************
:PROMPT
set /p veutContinuer=  �tes-vous certain de vouloir poursuivre? (O/[N])?
if /i "%veutContinuer%" neq "O" goto NON
echo **************************************************************************
rem a faire: ajouter un compteur de suppression (afficher le resultat a la fin)
rem Nettoyage des dossiers:
if exist Library rd /s /q Library
if exist Temp rd /s /q Temp
if exist obj rd /s /q obj
if exist .vscode rd /s /q .vscode
rem Nettoyage des fichiers:
if exist *.csproj del /s /q /f *.csproj
if exist *.DS_Store del /s /q /f *.DS_Store 
if exist *.sln del /s /q /f *.sln
if exist omnisharp.json del /s /q /f omnisharp.json
echo **************************************************************************
echo   Termin�. Le projet a �t� nettoy�.
echo **************************************************************************
goto FIN
:NON
echo **************************************************************************
echo   Le script ne sera pas ex�cut�.
echo **************************************************************************
:FIN
chcp %cp%>nul
pause