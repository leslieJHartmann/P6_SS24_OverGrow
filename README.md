# P6_SS24_OverGrow
Für Passant:innen an Unorten, die ein Bedürfniss
nach sozialer Verbundenheit haben und selbst
Orte bedeutsamer machen wollen, ist OverGrow
eine Ergänzung des Physischen Raumes, die soziale
Atmosphäre sichtbar macht und aktives Miteinander
sowie Kollaboration fordert.
Das System erweckt Unorte zum Leben, indem
es ein interaktives, virtuelles Ökosystem darstellt.
Kollaboration zwischen Nutzenden wird gefördert,
indem diese gemeinsam Farbpunkte sammeln, um
es gedeihen zu lassen.
Anstatt der sozialen Passivität bedeutungsloser
Nicht-Orte, wird in diesem Szenario sofort sichtbar
gemacht, wie sich das Miteinander auf die eigene
Umwelt und die Orte, in denen Menschen agieren,
auswirkt

# Technischer Prototyp
Der Technische Prototyp besteht aus einer Kamera und einem Projektor, der an eine Wand projeziert
Für den Prototypen wird nur Unity mit einem MediaPipe Plugin verwendet, da lediglich eine freie Fläche bespielt wird und Projection Mapping mit TouchDesigner an diesem Punkt noch nicht notwendig ist.

Die prototypische Umsetzung erarbeitet, die Art und Weise, wie ein, in der Projektion dargestelltes Objekt gegriffen und bewegt werden kann. Die Visualisierung steht hierbei im Hintergrund.

Das System erkennt eine offene oder geschlossene Hand.
Die geschlossene Hand, kann das dargestellte Objekt bewegen und ablegen, indem die Hand geöffnet wird.


# How-To
 
 ## Physische Teile
 Kamera und Beamer sollten an ähnlicher Position stehen, sodass die Projektion soweit möglich das komplette Kamerabild ausfüllt.
 Der Projektor dient als erweiterte Anzeigefläche des angeschlossenen Gerätes. Das Unity Game Fenster sollte hier im Vollbild geöffnet werden.

 ## Software
- Repository downloaden
- erforderliche Unity Version laden und öffnen
- Den Ordner Unity>OverGrow in Unity öffnen
- Unter Assets befindet sich TestAnimator. Diesen in Unity starten
- sicherstellen, dass im Animator Objekt die richtige Kamera ausgewählt ist
- "Play" drücken, um zu starten

Für ein optimales Ergebniss steht der Beamer etwa 1-2 Meter von der Wand entfernt, da die meisten Webcams über schlechte Bildqualität verfügen und die Gestenerkennung andernfalls nicht gewährleistet werden kann, und die Handgesten zum Testen werden etwa Mittig zwischen ihm und Projektionsfläche ausprobiert.


# Weiterführend
Ich würde gerne vor der Ausstellug und anschließend noch mehr Zeit in die Technik stecken und die Puzzle-Funktion mit den Lichtpunkten aus dem Konzept vollständig umsetzen bzw. das Hand-Tracking verbessern und optimieren, Grafiken und Modelle ein