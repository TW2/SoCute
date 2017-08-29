# SoCute
SUP to ASS converter
-
When you save your old DVD into your Hard Drive Disk (HDD) and there is a track with subtitles encoded in image, you feel unhappy.

The good things are the following :

1. Rip your DVD with DVD Decryter using demux for all of your tracks.
2. Use DGIndex to have a D2V for encoding later with AviSynth.
3. Use VobEdit to extract these tracks when they are in a VOB file.
4. Use SoCute to read your SUP subtitles file(s) and get an Advanced Sub Station (ASS) file of each SUP.
5. Use Aegisub to configure your render of Default style and get rid of errors.
6. Encode x264 or x265 video file from an AviSynth script.
7. Maybe use MKVToolsNix to mux all your files (video, audio and subtitle) with font(s).

SoCute is multilingual (using Tesseract 3.05 [you can change version by changing all files and directories in sub folder "tesseract"]).<br>
SoCute works from generated BMP of SUP Tools but transform it in black and white PNG to have a good image for tesseract-OCR.<br>
SoCute is slow but do the work.<br>
SoCute can be used for multiple SUP in one time, like a batch tool.<br>
SoCute is free and open source.<br>
SoCute thanks to everyone who have make software around DVD and OCR.

Tested in Visual Studio 2017 CE on a Windows 10 Pro US. Not tested outside VS, please be careful if you want to use it.
Clone or copy the zip and build it with Visual Studio 2017 Community Edition.

Source for Tesseract >> https://github.com/UB-Mannheim/tesseract/wiki
Source for SUP Tools >> https://sites.google.com/site/dvdsuptools/
