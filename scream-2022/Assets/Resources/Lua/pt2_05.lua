enterNVL()

if not getSwitch('pt2_05') then
	enter('SUMI', 'e', 'intense')
	speak('SUMI', "I don't care. I don't want to see any stories, clear? Not even in the student paper.")
	speak('SUMI', "Have them kill it on privacy grounds. Suicide, grieving family, whatever.")
	speak('SUMI', "...No. There won't be any followup. We have nothing to hide -- I just want this shut as soon as possible.")
	exit('sumi')
	enter('sumi', 'c')
	speak('SUMI', "Oh. You.")
	speak('ARIEL', "You're done with your calls?")
	speak('SUMI', "Yes. Just wrapping up some business from yesterday.")
	speak('ARIEL', "...Pardon me, but, how late were you here at Lucir offices last night?")
	speak('SUMI', "Me? Your colleague Dr. Kowalski said some extremely funny things about me, and then I left.")
	expr('sumi', 'intense')
	speak('SUMI', "How late were you here, Ariel?")
	speak('ARIEL', "I... don't really remember.")
	expr('sumi', 'surprise')
	speak('SUMI', "Izay no andrasana. Aza manahy momba izany.")
	expr('sumi', nil)
	speak('ARIEL', "I still don't get your jokes.")
	speak('SUMI', "Joke? I asked if we were ready to start the demo or not.")
else
	enter('sumi', 'c')
	speak('SUMI', "Are we ready to start the demo?")
end

setSwitch('pt2_05', true)

if not getSwitch('pt2_04') then
	speak('ARIEL', "I still need to talk to Noemi.")
else
	speak('ARIEL', "If I'm honest, we're probably not ready. But you insist, then I'll mirror for you.")
	speak('SUMI', "Ahaha. Have some confidence. Now let's get to the lab.")
	exitNVL()
	play('pt2_06')
end
