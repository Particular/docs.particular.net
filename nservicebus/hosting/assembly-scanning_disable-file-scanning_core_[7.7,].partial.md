### Disable assembly files scanning

Scanning of assemblies deployed to the `bin` folder (and other configured scanning locations) can be disabled:

snippet: disable-file-scanning

Warn: When disabling scanning of assembly files, ensure that all required assemblies are correctly loaded into the AppDomain at endpoint startup and that AppDomain assembly scanning is enabled.