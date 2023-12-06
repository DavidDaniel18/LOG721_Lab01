const { Web3 } = require('web3')
const fs = require('fs')
require('dotenv').config()

// Attention, il faut remplacer les valeurs selon votre config sur Ganache
const port = process.env.PORT || 7545
const contractJsonFilePath = process.env.CONTRACT_JSON_PATH
const contractAddress = process.env.CONTRACT_ADDRESS
const accountAddress = process.env.ACCOUNT_ADDRESS

const web3 = new Web3('http://localhost:' + port)
const contractJSON = JSON.parse(fs.readFileSync(contractJsonFilePath, 'utf-8'))
const studentContract = new web3.eth.Contract(contractJSON.abi, contractAddress)

async function setStudentNumber(studentNumber) {
  const gasEstimate = await studentContract.methods
    .setStudentNumber(studentNumber)
    .estimateGas({
      from: accountAddress,
      value: 5400000000000000,
    })
  const result = await studentContract.methods
    .setStudentNumber(studentNumber)
    .send({
      from: accountAddress,
      gas: gasEstimate,
      value: 5400000000000000,
    })

  console.log('Transaction hash:', result.transactionHash)
}

async function getStudentNumber() {
  const number = await studentContract.methods.getStudentNumber().call({
    from: accountAddress,
  })

  console.log('Student Number:', number)
}

// Attention, vu que c'est du async, il se peut que getStudentNumber fini avant addStudentNumber.
// C'était juste pour tester le comportement. Selon le chargé, on va réutilisé ce code dans la partie 4
setStudentNumber(20) // nbr aléatoire. On s'en fou
getStudentNumber()
