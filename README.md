# cc-artnet

ArtNet library for Unity(C#).

Forked from https://github.com/sugi-cho/ArtNet.Unity based on https://github.com/MikeCodesDotNET/ArtNet.Net

Receive and Send DMX512 via ArtNet.

## Usage

### Send DMX

```csharp
DMXController controller;
int universe;
byte[] dmxData;
//512 channels

controller.Send(universe, dmxData);
```
