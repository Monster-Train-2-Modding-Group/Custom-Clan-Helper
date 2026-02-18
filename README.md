# CustomClanHelper

[![GitHub Release](https://img.shields.io/github/v/release/Monster-Train-2-Modding-Group/Custom-Clan-Helper?color=4CAF50&label=latest)](https://github.com/Monster-Train-2-Modding-Group/Custom-Clan-Helper/releases)
[![Trainworks Reloaded](https://img.shields.io/badge/framework-Trainworks--Reloaded-blue?logo=github)](https://github.com/Monster-Train-2-Modding-Group/Trainworks-Reloaded)
[![License](https://img.shields.io/github/license/Monster-Train-2-Modding-Group/Custom-Clan-Helper?color=lightgrey)](https://github.com/Monster-Train-2-Modding-Group/Custom-Clan-Helper/blob/main/LICENSE)
[![Donate](https://img.shields.io/badge/Ko--Fi-brandonandzeus-F16061?color=F16061&logo=ko-fi&style=flat&labelColor=?color=4E4E4E&logoColor=FFFFFF)](https://ko-fi.com/brandonandzeus)

Logbook changes to support custom clans

![page](https://raw.githubusercontent.com/Monster-Train-2-Modding-Group/Custom-Clan-Helper/main/screenshots/page.png)
<sub>Logbook with every custom clan (as of repo's creation) + 1 more.</sub>

## Features
  * Fixes the logbook to add a 3rd page for custom clans. Only fixes the checklist page.

## Installation

It is **highly** recommended to use a mod manager (Thunderstore Mod Manager, Gale Mod Manager, or r2modman) to install this mod.
This mod does not depend on any other mod to function, however to get any benefit from installing you would need to install at least 1 custom clan mod.

## TODO
  * Champion Upgrade Page in logbook is wonky.
  * Artifact page needs pagination support.
  * Clan select screen in Run setup screens don't scale properly
  * Custom expanded checklist screen that scales with number of clans gracefully.

## Known issues
  * Why the 3rd page?
	* Due to how it was coded, it is very hard to modify a page in the logbook.
  * Why is the top row of items in the checklist page smooshed
	* This is due to how it was configured the bottom row are "crew clans", that is the clans you unlock after 6 pyre hearts, the ui is configured to split the clans across the two rows.
  * Why are the cards on the checklist page smooshed together
	* That is because certain clans have more than the 42 cards that can be mastered. The default setup for the UI doesn't really handle that too well.