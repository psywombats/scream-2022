
speak('ARIEL', "It's Ratthew... He's still not moving.")
speak('ARIEL', "Even if his brain's on overdrive, he's effectively dead.")
exitNVL()
wait(0.3)
rotateTo('noemi_final')
wait(0.5)

enterNVL()
enter('NOEMI', 'c')
speak('ARIEL', "Noemi, what drug exactly were you testing on these guys? A new drug from Chris?")
speak('NOEMI', "No... Just Bluepill.")
speak('ARIEL', "Then why did this happen only now?")
speak('NOEMI', "Hm hm. We started the mirroring tests recently. I think Ratilda and Ratricia both sent and received dreams.")
speak('NOEMI', "But. Ratthew and Ratilda are both fairly old. A long term effect?")
speak('ARIEL', "Is that possible?")
speak('NOEMI', "Find Chris. He would know.")
speak('NOEMI', "But you and I have took such huge doses back in the olds days, in the ward. And we're fine.")
speak('ARIEL', "...I wonder if that's really true.")
exitNVL()
setSwitch('got_rat', true)
play('finale_gather_all')
