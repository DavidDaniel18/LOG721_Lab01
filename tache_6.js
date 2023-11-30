const { Web3 } = require('web3');
const fs = require('fs');

// Attention, il faut remplacer les valeurs selon votre config sur Ganache
const port = 7545
const contractJsonFilePath = './studentNumberContract/build/contracts/StudentContract.json'
const contractAddress = '0x76a601098DE51517BAE4FD23C67F4809Ac619Bf4';
const accountAddress = '0xb1568FcB07DBDd009f09f4C4254065470B6b810B';


const web3 = new Web3('http://localhost:' + port);
const contractJSON = JSON.parse(fs.readFileSync(contractJsonFilePath, 'utf-8'));
const studentContract = new web3.eth.Contract(contractJSON.abi, contractAddress);


async function setStudentNumber(studentNumber) {
    const gasEstimate = await studentContract.methods.setStudentNumber(studentNumber).estimateGas({
        from: accountAddress,
        value: 5400000000000000
    });
    const result = await studentContract.methods.setStudentNumber(studentNumber).send({
        from: accountAddress,
        gas: gasEstimate,
        value: 5400000000000000
    });

    console.log('Transaction hash:', result.transactionHash);
}

async function getStudentNumber() {
    const number = await studentContract.methods.getStudentNumber().call({
        from: accountAddress
    });

    console.log('Student Number:', number);
}

// Attention, vu que c'est du async, il se peut que getStudentNumber fini avant addStudentNumber.
// C'était juste pour tester le comportement. Selon le chargé, on va réutilisé ce code dans la partie 4
setStudentNumber(20); // nbr aléatoire. On s'en fou
getStudentNumber();
