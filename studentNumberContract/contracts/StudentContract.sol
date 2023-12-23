// SPDX-License-Identifier: MIT 
pragma solidity >=0.5.0;
contract StudentContract {
    uint256 public studentNumber ;
    address public student ;
    mapping ( address => uint256 ) studentToStudentNumber ;
    constructor () public {
        student = msg . sender ;
        studentToStudentNumber [ student ] = 0;
    }
    function setStudentNumber ( uint256 _studentNumber ) public payable {

        require(msg.value == 5400000000000000, "Vous devez specifier 5400000000000000 wei");

        studentNumber = _studentNumber ;
        studentToStudentNumber [ student ] = _studentNumber ;
    }

    function getStudentNumber ( ) public view returns (uint256) {
        return studentNumber;
    }
}