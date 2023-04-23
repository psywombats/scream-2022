teleport('Recurse', 'chair', 'SOUTH')

wait(0.5)
speak('Ariel', "Let's bring Recurse online...")
wait(0.3)
bootRecurse(true)
wait(0.5)

enterNVL()
speak('ARIEL', "And begin. It's time to record my thoughts.")
speak('ARIEL', "If I'm right about this, then...")
speak('ARIEL', "I was unwell yesterday. And for a long time before that. My brain started working in overdrive, just like...")

if not clue('rat') then
	play('finale_quit')
	return
end

speak('ARIEL', "...the poor rats. Either because of my experiences in Ward No.9, or my later experimentation with Recurse, I was on a timer. I lost my ability to keep my nightmares in check.")
speak('ARIEL', "I tried to run from those old horrors that bled into my perception. But, my ultimate fate is detailed here:")

if not clue('pendant') then
	play('finale_quit')
	return
end

speak('ARIEL', "I died. I am dead. Indirectly from Bluepill, but more directly by from hurling myself out of the tower.")
speak('ARIEL', "I thought it was a dream, but I was wide awake. I fell over thirty stories and died.")
speak('ARIEL', "Everyone probably considers it a suicide, and someone like Sumi would be able to cover up most of the bad press.")
speak('ARIEL', "Braulio probably took the pendant and article as keepsakes to remember me.")
speak('ARIEL', "But... I'm still here. Or... I think I am. I know things only Ariel could know. I have the memories of Ariel.")
speak('ARIEL', "But I think it's more accurate to say, I have a fragmented memory of Ariel's last day alive. I only believe those fragments to be my memories because of...")

if not clue('report') then
	play('finale_quit')
	return
end

speak('ARIEL', "...oneirophrenia. Me, whoever I am, dosed myself so high and saw so many of Ariel's dreams, that I believed those dreams were real. And as I'd experienced ego death, I fashioned Ariel's identity into my own.")
speak('ARIEL', "Of course, I conveniently forgot that anything like that was possible. Chris could never tell me because of...")

if not clue('laptop') then
	play('finale_quit')
	return
end

speak('ARIEL', "...the tampering software. Probably Braulio clumsily deleted every mention of oneirophrenia from Ariel's dreams so I wouldn't discover what really happened.")
speak('ARIEL', "That didn't work too well. I could tell something was missing.")
speak('ARIEL', "If I'm not Ariel, though... I can only think of one person who would try to save my dreams even after my death...")

if not clue('diary') then
	play('finale_quit')
	return
end

speak('ARIEL', "...Dr. Kowalski. I'm Dr. Christian Kowalski. Or at least my body is. He must've erased himself with Bluepill.")
speak('ARIEL', "He figured out that with Bluepill-induced oneirophrenia, high-quality dreams could be interpreted as memory.")
speak('ARIEL', "And he tried to tell me, even if Braulio erased those bits.")
speak('ARIEL', "It's clear from his dream diary that he was depressed and still felt guilty about his role in the trauma that Noemi and I experienced.")
speak('ARIEL', "He'd be willing to destroy his personality if it meant a chance for me to live on in some way.")
speak('ARIEL', "Noemi's near-blindness prevented her from figuring out the swap. But why would Braulio and Sumi play along?")
speak('ARIEL', "Braulio probably followed Sumi's lead. In fact, I think this whole thing might've been her idea.")
speak('ARIEL', "The real Sumi Chey is a very rich woman, and a very old woman. If she somehow got it into her head that she could use Bluepill to pour dreams or memories into another body, the Sumi we know is actually...")

if not clue('photo') then
	play('finale_quit')
	return
end

speak('ARIEL', "...Sara Andrianami. That's her real name. Sumi, or Sara, was no stranger to Bluepill dreams. She even said so to Braulio.")
speak('ARIEL', "There are plenty of unethical labs overseas experimenting with Bluepill. Even some here, like the one that ran the hospital with Noemi and me.")
speak('ARIEL', "At one of those underground labs, Sumi Chey must've tried to erase Sara's identity and replace it with her own.")
speak('ARIEL', "I doubt it was consensual. Did Sumi Chey just pick someone that matched her younger self? It seems like Sara may not have even spoken English.")
speak('ARIEL', "And the overwrite clearly wasn't clean. Part of her is still there, trying to fight back in Sumi's dreams, and might even be awake now, and very disturbed.")
speak('ARIEL', "If the overwriting was incomplete, of course the real Sumi would be looking for another lab to try it again, but more thoroughly. And so she found Lucir.")
speak('ARIEL', "This whole dream swap, overwriting Chris with me... It's a test run. Sumi wanted to gauge just how effectively we'd be able to perform her trick.")
speak('ARIEL', "And so, once again, I am a lab rat.")

exitNVL()

speak('ARIEL', "The pieces fit. This concludes my lucid dream of March 2nd. In this dream, I wander the Lucir office and gather the evidence to support my theory.")
bootRecurse(false)
speak('ARIEL', "All that remains is for me to wake up.")
speak('ARIEL', "...to wake up.")
fade('black')
speak('ARIEL', "I need to wake up.")
setSwitch('finale_mode', true)
fade('normal')




