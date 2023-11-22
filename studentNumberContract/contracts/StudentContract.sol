pragma solidity >=0.5.0;
contract StudentContract {
    uint public studentNumber ;
    address public student ;
    mapping ( address => uint ) studentToStudentNumber ;
    constructor () public {
        student = msg . sender ;
        studentToStudentNumber [ student ] = 0;
    }
    function setStudentNumber ( uint _studentNumber ) public {
        studentNumber = _studentNumber ;
        studentToStudentNumber [ student ] = _studentNumber ;
    }
}