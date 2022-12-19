/*query per avere tutti i nomi dei gruppi che sono in contatto con pippo*/
select distinct c.titolo
from (utentichat as uc join login as lo on uc.idUtente=lo.id) join chat as c on uc.idChat=c.idChat  
where not c.titolo=""and  not lo.user="pippo" and uc.idChat in (select uc.idChat
										from (utentichat as uc join login as lo on uc.idUtente=lo.id) join chat as c on uc.idChat=c.idChat  
										where user="pippo");
										
/*query per avere tutti i nomi dei singoli che sono in contatto con pippo*/
select user
from (utentichat as uc join login as lo on uc.idUtente=lo.id) join chat as c on uc.idChat=c.idChat  
where not lo.user="pippo" and uc.idChat in (select uc.idChat
										from (utentichat as uc join login as lo on uc.idUtente=lo.id) join chat as c on uc.idChat=c.idChat  
										where user="pippo" and c.titolo="");