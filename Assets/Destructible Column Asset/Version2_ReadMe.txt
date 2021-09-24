Thank you for downloading version 2 of my destructable column asset. This update adds a new varient of the 
column called the "Column Destructable Variable Prefab" which give each breakable part of the column a healthpoint
variable. When these pieces are collided with, they check to see if the colliding object is tagged as "Damaging". 
If the object is "Damaging" then the column piece will take damage equal to the Damage Output value of the 
"Damaging Object". Once the column piece's healthpoints reach zero, the piece will fall away from the main column.

To set an object as "Damaging":
	1. Set the object's tag to "Damaging"
	2. Ensure it has a collider and rigidbody attached
	3. Attach the "DamageOutputScript" script to the object
	4. Set the damage output value in the inspector