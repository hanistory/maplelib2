NOTE: as of rev 22 you need the NAudio library to compile this. You can get NAudio from http://naudio.codeplex.com/ (their binaries are outdated, get the source from the SVN) or from my other project, HaSuite - http://code.google.com/p/hasuite/

The continuation by haha01haha01 of a general-purpose MapleStory library coded by Snow which utilizes a fork of the WZ library coded by Jonyleeson which is based on algorithms coded by various other people.

After Snow quit developing, I took over the project out of my own will because I had to improve it a lot before it is suitable to be used in my repacker\editor. Here, I am going to post changes and improvements to the library. I probably won't touch the packet library part because I am focusing on WZ tools, but suggestions and code pieces you think should be added are always welcomed (you can contact me via my email).

Also, notice that since it's my version the "Changed" images method has returned (wzImage.Changed must be set to true in order for wzImage's changes to be saved). I know that it may be annoying when developing, but it pays back in saving times.