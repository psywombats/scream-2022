
enterNVL()

speak('CHRIS', "Ariel. Ariel! Are you alright?")
speak('NOEMI', "Hmmm. These delta waves... There's a flowing rhythm in them.")
setWake(1)
wait(2)
speak('CHRIS', "Does that mean Ariel's still asleep?")
speak('NOEMI', "Hm hm. Maybe and maybe not.")
setWake(2)
wait(2)
speak('ARIEL', "I'm awake. Sorry.")
exitNVL()

setWake(-1)
wait(2)
setting("March 1st, 11:00AM")
setting("Aquila Tower F37")
setting("Lucir Offices")
setSwitch('no_settings', false)
wait(3)

enterNVL()
enter('CHRIS', 'b')
enter('NOEMI', 'd')
speak('CHRIS', "There's no need to apologize. Did something happen? Your heart was beating so fast that I was afraid I'd overdosed you.")
speak('ARIEL', "Nothing happened.")
speak('NOEMI', "Whatever you were dreaming, I couldn't record it. Recurser didn't pick it up... Too bad.")
speak('ARIEL', "It wasn't a lucid dream. You wouldn't have wanted to record it anyway.")
speak('CHRIS', "Maybe it's a problem with the new formula... I'll change the balance back for next time.")
speak('NOEMI', "Or a bug with Recurser. I'm not perfect.")
speak('ARIEL', "Just a bug...")
exitNVL()

wait(1.5)
rotateTo('braulio_pt1')
wait(1.3)

setSwitch('pt1_02', true)
play('pt1_03')

