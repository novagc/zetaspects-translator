# ZetAspects Translator

## About

This mod translates the [ZetAspects](https://thunderstore.io/package/William758/ZetAspects/) mod.
Russian localisation is supplied by default.
---
Данный мод переводит [ZetAspects](https://thunderstore.io/package/William758/ZetAspects/).
По умолчанию поставляется русская локализация.

## Translations

To add your localisation, place the appropriate settings in one of the 2 directories:
1. `BepInEx\plugins\NovaGC-ZetAspectsTranslator\NovaGC-ZetAspectsTranslator\translations`
2. `Risk of Rain 2_Data\StreamingAssets\LanguageOverrides\ZetAspects`

> It is **recommended** to use the **2nd option**, as the localisation in the 1st directory **may be deleted** when updating the mod

> If the directory is missing, start the game with the mod enabled. All necessary directories and files will be created

Localisation is set using a `configuration` json file and a `tokens` json file *(optional)*.

> The token files **MUST** be placed in a separate directory `./tokens` or any directory other than the configuration directory

### Format

All localization json files must match their [schemas](https://json-schema.org/) for the mod to work correctly:
1. `settings.schema.json` *- for configurations*
2. `tokens.schema.json` *- for tokens*

These files are located in the directories where the localization files are stored.

To apply the schema to your json file, add a `$schema` field to the **root object**.

For example:
```json
{
   "$schema": "./settings.schema.json"
}
```

#### Tokens file

Represents 1 object where **fields are tokens** and **values are translations**.

For example:
```json
{
   "$schema": "./tokens.schema.json",
   "AFFIX_RED_NAME": "Ifrit's Distinction",
   "AFFIX_RED_PICKUP": "Become an aspect of fire."
}
```

#### Configuration file

`Root object`:

| Field        | Optional | Description                                                                      |
|:-------------|:--------:|----------------------------------------------------------------------------------|
| **language** |    -     | Localization language                                                            |
| **tokens**   |    +     | A list of localization tokens.<br/>_The structure is similar to the tokens file_ |
| **extends**  |    +     | List of localization dependencies                                                |

Field `extends`:

| Field    | Optional | Default  | Description                           |
|:---------|:--------:|:--------:|---------------------------------------|
| **path** |    -     |    -     | Relative or absolute path to the file |
| **type** |    +     | `tokens` | Dependency type:`setting` or `tokens` |

For example:

```json
{
   "$schema": "./settings.schema.json",
   "language": "en",
   "tokens": {
      "AFFIX_RED_NAME": "Ifrit's Distinction",
      "AFFIX_RED_PICKUP": "Become an aspect of fire."
   },
   "extends": [
      {
         "path": "./tokens/eng.base.json"
      },
      {
         "path": "./tokens/eng.items.json",
         "type": "tokens"
      },
      {
         "path": "./default.json",
         "type": "setting"
      }
   ]
}
```

### Overwrites

**Tokens files** and **other configuration files** can be used to set up localization in the configuration file by specifying them in the `extends` field.

If you specify **other configuration file** in the dependencies, it will recursively load **all the tokens** it provides directly from the `tokens` field and from its `extends` field.

> You can use other configurations regardless of what language they are made for. Only tokens are loaded.

If the same token is downloaded from different sources, the one with the highest priority will be used.

#### Priority

The `tokens` field has **the highest** priority.

Next are the dependencies from the `extends` field. The priority of a dependency is determined by **its order in the list**.

> The 1st dependency has a higher priority than the 2nd one, the 2nd one has a higher priority than the 3rd one, etc.

Localizations from the `Risk of Rain 2_Data\StreamingAssets\LanguageOverrides\ZetAspects` directory generally have higher priority than localizations from `BepInEx\plugins\NovaGC-ZetAspectsTranslator\NovaGC-ZetAspectsTranslator\translations`, so they will always overwrite tokens.

If there are multiple localizations for the same language, higher priority will be given according to **alphabetical sorting**.

> For example, `a.json` > `b.json`

## Build

> After the project is built, all files are collected in the `public` directory.

### Rider

1. Open `ZetAspectsTranslator.sln`
2. Add Reference to `ZetAspects`:
   1. In "Solution" tab add a reference to `ZetAspects` to the `ZetAspectsTranslator` project.

      > `ZetAspectsTranslator` > `ZetAspectsTranslator` > `Dependencies` > `.NETStandard 2.1` > `Assemblies` > `Reference...` > `Add From...`
   2. Select path to `ZetAspects.dll`.
   
      > `BepInEx\plugins\William758-ZetAspects\ZetAspects\ZetAspects.dll`
   3. **Uncheck** `Copy local` in `ZetAspects` reference
3. Build solution

### Visual Studio

The steps for Visual Studio are similar