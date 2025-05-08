# openTAG Funnel 🏷️

A reference implementation for the openTAG digital product passport system. Built with .NET MAUI.

## Overview 🔍

openTAG Funnel connects to the openTag ecosystem to help you manage digital product passports. When you start the application, it asks for your JWT token and template ID associated with your organization. You can then enter an openTAG ID directly or a valid openTAG URL, which prompts the application to fetch the corresponding template and data.

The interface displays all template fields, pre-filling any default values where available. After entering your information and saving, the application creates a new openTAG ID data package and submits it to the API.

## Building the Application 🛠️

### Windows 💻

```
# Install .NET 9 SDK
winget install Microsoft.DotNet.SDK.9

# Install required MAUI workloads
dotnet workload install maui

# Clone the repository
git clone https://github.com/GluetexGmbH/opentag-funnel.git
cd opentag-funnel

# Build the application
dotnet build OpenTAG.Funnel.slnx

# Run the application
dotnet run --project OpenTAG.Funnel/OpenTAG.Funnel.csproj
```

For Visual Studio users, simply open the solution file and run it with F5 after selecting Windows as the target.

### Android 📱

```
# Install .NET 9 SDK if not already installed
winget install Microsoft.DotNet.SDK.9

# Install required MAUI and Android workloads
dotnet workload install maui android android-build

# Clone the repository
git clone https://github.com/GluetexGmbH/opentag-funnel.git
cd opentag-funnel

# Build the Android application
dotnet build OpenTAG.Funnel.slnx -f net9.0-android

# Create the APK
dotnet publish OpenTAG.Funnel/OpenTAG.Funnel.csproj -f net9.0-android -c Release
```

The APK will be located in the bin/Release/net9.0-android directory.

### macOS 🍎

```
# Install Xcode from the App Store first

# Install .NET 9 SDK
brew install dotnet-sdk-9.0

# Install required MAUI workloads
dotnet workload install maui macos maccatalyst

# Clone the repository
git clone https://github.com/GluetexGmbH/opentag-funnel.git
cd opentag-funnel

# Build the application
dotnet build OpenTAG.Funnel.slnx -f net9.0-maccatalyst

# Run the application
dotnet run --project OpenTAG.Funnel/OpenTAG.Funnel.csproj -f net9.0-maccatalyst
```

Open in VSCode with `code .` to make any customizations before building.

## Configuration ⚙️

Before using openTAG Funnel, you need a valid JWT token from GlueTex GmbH and your organization's template ID. The application will prompt you for these values during first launch.

## Development Notes 📝

Built with .NET MAUI on .NET 9, this application works across Windows, Android, and macOS without external dependencies. The codebase follows standard MAUI architecture with platform-specific code kept to a minimum.

## License 📄

This project is licensed under the MIT License - see the LICENSE file for details.