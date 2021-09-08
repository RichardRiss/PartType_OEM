DELIMITER $$
DROP FUNCTION IF EXISTS `update_description` $$
CREATE DEFINER=`root`@`localhost` FUNCTION `update_description` (wptype json, wpname json)
	RETURNS BOOL DETERMINISTIC
BEGIN
	DECLARE _name VARCHAR(255);
    DECLARE _type INT;
	DECLARE _counter INT;
	DECLARE func_ret BOOL DEFAULT False;
    
    SET func_ret = IF(JSON_LENGTH(wpname) = JSON_LENGTH(wptype) = 1000, False, True);
    IF func_ret = False THEN
		WHILE _counter < JSON_LENGTH(wptype) DO
			SET _name = JSON_UNQUOTE(JSON_EXTRACT(wpname , CONCAT('$[', _counter, ']')));
			SET _type = JSON_UNQUOTE(JSON_EXTRACT(wptype , CONCAT('$[', _counter, ']')));
			UPDATE IGNORE workpieces_description
				SET description_deu = _name
			WHERE wptype = _type;
			SET _counter =_counter + 1;
		END WHILE;
    END IF;
	return func_ret;
END $$

DELIMITER ;