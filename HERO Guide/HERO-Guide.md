# HERO-Guide

This is the guide for the HERO Board by CTRE. It will cover installation, basic code, and helpful sites. Most of these information can be found on the User Guide for the HERO Board [here](https://www.ctr-electronics.com/downloads/pdf/HERO%20User's%20Guide.pdf). The guide will refer back to it often, so have it opened would help clarify the instructions. It is recommended to at least skim through everything to get a good of idea of how the HERO Board works.

## Installation

1. Download and install CTRE Phoenix Framework [here](https://www.ctr-electronics.com/hro.html#product_tabs_technical_resources).
2. Download and install Visual Studio Community 2019 [here](https://visualstudio.microsoft.com/vs/).
   * At somepoint in the process, an image like below would appear. Select the box for .NET desktop development.
    ![Alt](/img/VS19_select_workloads.JPG)
3. Install the VSIX (extension for the libraries)
   * It should be located at ```C:\Users\Public\Documents\Cross The Road Electronics\HERO\Visual Studio Plugin```
   * Install the latest version by double clicking

## HERO Lifeboat

This is a tool to update the HERO Board and any other CAN Devices connected to the system. Refer to Section 7 for how to use the Lifeboat

## Getting Started

To create a project, follow Section 8 of the User Guide. It covers how to write some basic code and points out a few important facts. Below are some key facts to know for this section.

* After the project is created, navigate to the folder where the project is saved (the folder with the .sln file). Create a new txt document and rename it to Directory.Build.props. Open it put these lines in.

    ``` xml
    <Project>
        <PropertyGroup>
            <LangVersion>5</LangVersion>
        </PropertyGroup>
    </Project>
    ```

    The reason for this is because Visual Studio 2019 automatically chooses the newest version of C#, but HERO is using an older version, so you have to set the version manually

* The line ```CTRE.Phoenix.Watchdog.Feed();``` is used as a safety feature for motors. If it is not called, all motors will stop. Figure out a way to implement this to fit your needs. An example is shown below:

    ``` C#
    if (myGamepad.GetConnectionStatus() == CTRE.Phoenix.UsbDeviceConnection.Connected)
    {
        CTRE.Phoenix.Watchdog.Feed();
    }
    ```

    This example checks if a USB controller ```myGamepad``` is connected, then the watchdog will be fed.

## Helpful Sites

HERO User Guide: <https://www.ctr-electronics.com/downloads/pdf/HERO%20User's%20Guide.pdf>

HERO Example codes: <https://github.com/CrossTheRoadElec/Phoenix-Examples-Languages/tree/master/HERO%20C%23>

netMF Library: <https://github.com/CrossTheRoadElec/Phoenix-netmf/tree/master/CTRE>
